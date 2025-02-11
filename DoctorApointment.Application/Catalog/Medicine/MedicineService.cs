﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorApointment.Application.Common;
using DoctorApointment.Data.EF;
using DoctorApointment.Data.Entities;
using Microsoft.AspNetCore.Http;
using DoctorApointment.ViewModels.Catalog.Medicine;
using DoctorApointment.ViewModels.Common;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;

namespace DoctorApointment.Application.Catalog.Medicine
{
    public class MedicineService : IMedicineService
    {
        private readonly DoctorManageDbContext _context;
        private readonly IStorageService _storageService;
        private const string MEDICINE_CONTENT_FOLDER_NAME = "medicine-content";
        public MedicineService(DoctorManageDbContext context,
            IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<ApiResult<bool>> Create(MedicineCreateRequest request)
        {
            var medicine = new Medicines()
            {
                Description = request.Description,
                IsDeleted = false,
                Image = await SaveFile(request.Image, MEDICINE_CONTENT_FOLDER_NAME),
                CreatedAt = DateTime.Now,
                Name = request.Name,
                Price = request.Price,
                ParentId = request.ParentId,
                Unit = request.Unit,

            };
            _context.Medicines.Add(medicine);
            var rs = await _context.SaveChangesAsync();
            if (rs != 0)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Tạo đơn thuốc không thành công!");
        }
        private async Task<string> SaveFile(IFormFile? file, string folderName)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsyncs(file.OpenReadStream(), fileName, folderName);
            return fileName;
        }
        public async Task<ApiResult<int>> Delete(Guid Id)
        {
            var medicines = await _context.Medicines.FindAsync(Id);
            int check = 0;
            if (medicines == null) return new ApiSuccessResult<int>(check);
            if (medicines.IsDeleted == false)
            {
                medicines.IsDeleted = true;
                check = 2;

            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int>(check);
        }

        public async Task<ApiResult<List<MedicineVm>>> GetAll(string UserName)
        {
            var usser = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == UserName);
            var doctor = await _context.Doctors.FindAsync(usser.Id);
            var parentId = doctor.ClinicId == null ? doctor.UserId : doctor.ClinicId;
            var query = _context.Medicines.Where(x => x.IsDeleted == false && x.ParentId == parentId);

            var rs = await query.Select(x => new MedicineVm()
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                Price = x.Price,
                IsDeleted = x.IsDeleted,
                Name = x.Name,
                ParentId = x.ParentId,
                Description = x.Description,
                Image = MEDICINE_CONTENT_FOLDER_NAME + "/" + x.Image,
                Unit = x.Unit,
            }).ToListAsync();
            return new ApiSuccessResult<List<MedicineVm>>(rs);
        }

        public async Task<ApiResult<PagedResult<MedicineVm>>> GetAllPaging(GetMedicinePagingRequest request)
        {
            var usser = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == request.UserName);
            var doctor = await _context.Doctors.FindAsync(usser.Id);
            var parentId = doctor.ClinicId == null ? doctor.UserId : doctor.ClinicId;
            var query = from m in _context.Medicines
                        where m.ParentId == parentId
                        select m;
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new MedicineVm()
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    Price = x.Price,
                    IsDeleted = x.IsDeleted,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    Description = x.Description,
                    Unit = x.Unit,
                    Image = MEDICINE_CONTENT_FOLDER_NAME + "/" + x.Image,
                }).ToListAsync();

            var pagedResult = new PagedResult<MedicineVm>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<MedicineVm>>(pagedResult);
        }

        public async Task<ApiResult<MedicineVm>> GetById(Guid Id)
        {
            var x = await _context.Medicines.FindAsync(Id);
            if (x == null) return new ApiErrorResult<MedicineVm>("Đơn thuốc không được xác nhận!");
            var rs = new MedicineVm()
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                Price = x.Price,
                IsDeleted = x.IsDeleted,
                Name = x.Name,
                ParentId = x.ParentId,
                Description = x.Description,
                Image = MEDICINE_CONTENT_FOLDER_NAME + "/" + x.Image,
                Unit = x.Unit,
            };
            return new ApiSuccessResult<MedicineVm>(rs);
        }

        public async Task<ApiResult<bool>> Update(MedicineUpdateRequest request)
        {
            var medicines = await _context.Medicines.FindAsync(request.Id);
            if (medicines == null) return new ApiErrorResult<bool>("Đơn thuốc không được xác nhận!");

            medicines.Description = request.Description;
            medicines.Name = request.Name;
            medicines.Price = request.Price;
            medicines.Unit = request.Unit;
            medicines.IsDeleted = request.IsDeleted;
            if (request.Image != null)
            {
                if (medicines.Image != null && medicines.Image != "default") await _storageService.DeleteFileAsyncs(medicines.Image, MEDICINE_CONTENT_FOLDER_NAME);
                medicines.Image = await SaveFile(request.Image, MEDICINE_CONTENT_FOLDER_NAME);
            }

            var rs = await _context.SaveChangesAsync();
            if (rs != 0) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Cập nhật đơn thuốc không thành công!");
        }
    }
}