﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorApointment.Data.EF;
using DoctorApointment.Data.Entities;
using DoctorApointment.Utilities.Exceptions;
using DoctorApointment.ViewModels.Catalog.Location;
using DoctorApointment.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace DoctorApointment.Application.Catalog.Location
{
    public class LocationService : ILocationService
    {
        private readonly DoctorManageDbContext _context;

        public LocationService(DoctorManageDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> Create(LocationCreateRequest request)
        {
            var locations = new Locations()
            {
                Name = request.Name,
                SortOrder = request.SortOrder,
                ParentId = request.ParentId,
                Code = request.Code,
                IsDeleted = false,
                Type = request.Type
            };
            _context.Locations.Add(locations);
            var rs = await _context.SaveChangesAsync();
            if (rs != 0) return new ApiSuccessResult<bool>(true);
            return new ApiSuccessResult<bool>(false);
        }

        public async Task<ApiResult<int>> Delete(Guid Id)
        {
            var locations = await _context.Locations.FindAsync(Id);
            if (locations == null) return new ApiSuccessResult<int>(0);

            locations.IsDeleted = true;

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<int>(2);
        }

        public async Task<ApiResult<List<LocationVm>>> GetAllSubDistrict(Guid districtId)
        {
            var query = _context.Locations.Where(x => x.ParentId == districtId && x.Type.ToUpper() == "SUBDISTRICT");

            var rs = await query.Select(x => new LocationVm()
            {
                Id = x.Id,
                Name = x.Name,
                SortOrder = x.SortOrder,
                ParentId = x.ParentId
            }).ToListAsync();
            return new ApiSuccessResult<List<LocationVm>>(rs);
        }
        public async Task<ApiResult<List<LocationVm>>> GetAllDistrict(Guid provinceId)
        {
            var query = _context.Locations.Where(x => x.ParentId == provinceId && x.Type.ToUpper() == "DISTRICT");

            var rs = await query.Select(x => new LocationVm()
            {
                Id = x.Id,
                Name = x.Name,
                SortOrder = x.SortOrder,
                ParentId = x.ParentId
            }).ToListAsync();
            return new ApiSuccessResult<List<LocationVm>>(rs);
        }
        public async Task<ApiResult<List<LocationVm>>> GetAllProvince()
        {
            var query = _context.Locations.Where(x => x.Type.ToUpper() == "PROVINCE");

            var rs = await query.Select(x => new LocationVm()
            {
                Id = x.Id,
                Name = x.Name,
                SortOrder = x.SortOrder,
                ParentId = x.ParentId
            }).ToListAsync();
            return new ApiSuccessResult<List<LocationVm>>(rs);
        }
        public async Task<ApiResult<List<LocationVm>>> GetAllDistrictDaNangCity()
        {
            var danangCity = await _context.Locations.FirstOrDefaultAsync(x => x.Name == "Thành phố Đà Nẵng");
            var query = _context.Locations.Where(x => x.ParentId == danangCity.Id && x.Type.ToUpper() == "DISTRICT");

            var rs = await query.Select(x => new LocationVm()
            {
                Id = x.Id,
                Name = x.Name,
                SortOrder = x.SortOrder,
                ParentId = x.ParentId
            }).ToListAsync();
            return new ApiSuccessResult<List<LocationVm>>(rs);
        }
        public async Task<ApiResult<PagedResult<LocationVm>>> GetAllPaging(GetLocationPagingRequest request)
        {
            var query = from c in _context.Locations select c;
            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword));
            }
            if (!string.IsNullOrEmpty(request.Type) && request.Type.ToUpper() != "ALL")
            {
                query = query.Where(x => x.Type.ToUpper() == request.Type.ToUpper());
            }
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new LocationVm()
                {
                    Name = x.Name,
                    SortOrder = x.SortOrder,
                    Id = x.Id,
                    ParentId = x.ParentId,
                    Code = x.Code,
                    IsDeleted = x.IsDeleted,
                    Type = x.Type
                }).ToListAsync();

            var pagedResult = new PagedResult<LocationVm>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<LocationVm>>(pagedResult);
        }

        public async Task<ApiResult<LocationVm>> GetById(Guid Id)
        {
            var Locations = await _context.Locations.FindAsync(Id);
            if (Locations == null) throw new DoctorManageException($"Cannot find a location with id: {Id}");
            var rs = new LocationVm()
            {
                Id = Locations.Id,
                Name = Locations.Name,
                SortOrder = Locations.SortOrder,
                ParentId = Locations.ParentId,
                IsDeleted = Locations.IsDeleted,
                Code = Locations.Code,
                Type = Locations.Type
            };

            return new ApiSuccessResult<LocationVm>(rs);
        }

        public async Task<ApiResult<bool>> Update(LocationUpdateRequest request)
        {
            var Locations = await _context.Locations.FindAsync(request.Id);
            if (Locations == null) return new ApiSuccessResult<bool>(false);
            Locations.Name = request.Name;
            Locations.SortOrder = request.SortOrder;
            Locations.ParentId = request.ParentId;
            Locations.IsDeleted = request.IsDeleted;
            Locations.Code = request.Code;
            Locations.Type = request.Type;

            var rs = await _context.SaveChangesAsync();
            if (rs != 0) return new ApiSuccessResult<bool>(true);
            return new ApiSuccessResult<bool>(false);
        }

        public async Task<ApiResult<List<LocationVm>>> GetListAllPaging(string? type)
        {
            var query = from l in _context.Locations select l;
            //2. filter

            if (!string.IsNullOrEmpty(type) && type.ToUpper() != "ALL")
            {
                query = query.Where(x => x.Type.ToUpper() == type.ToUpper());
            }
            var rs = await query.Select(x => new LocationVm()
            {
                Name = x.Name,
                SortOrder = x.SortOrder,
                Id = x.Id,
                ParentId = x.ParentId,
                Code = x.Code,
                IsDeleted = x.IsDeleted,
                Type = x.Type
            }).ToListAsync();
            return new ApiSuccessResult<List<LocationVm>>(rs);
        }
    }
}
