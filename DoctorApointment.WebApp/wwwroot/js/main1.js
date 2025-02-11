const formElement = document.querySelector('#change-pass') 
const passwordInputs = formElement.querySelectorAll('[type=password]')
const fullNameInput = formElement.querySelector('[name=fullName]')
const passwordInput = formElement.querySelector('[name=Password]')
const formCheckList = formElement.querySelector('.form-section__form-checkList')
const formCheckItems = formCheckList.querySelectorAll('.form-section__form-checkItem')
const toggleShowPasswordBtn = formElement.querySelector('.form-section__form-group-icon')
const avatarInput = formElement.querySelector('#avatar')
const avatarImage = formElement.querySelector('.form-section__form-show-img')
const linkElement = document.querySelector('.form-section__footer-link')
const provinceElement = document.querySelector('#province')
const favoriteElement = document.querySelector('.form-section__favorite-list')

document.addEventListener("DOMContentLoaded", () => {

	const replaceAllRegex = (regex, element, alphaReplace) => {
			element.value =
				element.value.replaceAll(regex, alphaReplace)
	}

	const convertCambelCase = (char, string) => {
		string.replaceAll(char, char.toUpperCase())
	}


	// Add / remove class in password input
	passwordInput.onfocus = () => {
		if(formCheckList.classList.contains('hidden'))
			formCheckList.classList.remove('hidden')
	}

	// Validate password
	passwordInput.addEventListener('input', () => {
		const regexs = [
			/.{8,}/,
			/(?=(.*[A-Z]))/,
			/(?=.*[a-z])/gm,
			/(?=(.*[0-9]))/gm,
			/(?=.*[\!@#$%^&*()\\[\]{}\-_+=~`|:;"'<>,./?])/
		]

		regexs.forEach((regex, index) => {
			replaceAllRegex(/ /g, passwordInput, '')
			if(regexs[index].test(passwordInput.value)) {
				if(!formCheckItems[index].classList.contains('checked'))
				formCheckItems[index].classList.add('checked')
			}
			else {
				formCheckItems[index].classList.remove('checked')
			}
		})

	})

	// show / hidden password
	
		toggleShowPasswordBtn.onclick = () => {
			if(toggleShowPasswordBtn.classList.contains('hiden')) {
				toggleShowPasswordBtn.classList.remove('hiden')
				passwordInput.type = "text"
			}
			else {
				toggleShowPasswordBtn.classList.add('hiden')
				passwordInput.type = "password"
			}
		}


	// Validate form register
	Validator({
		form: "#change-pass",
		formGroup: ".form-section__form-group",
		errorMessage: ".form-section__form-message",
		rules: [
			//Validator.isRequired('#fullName', 'Vui lòng nhập họ tên'),
			//Validator.minLength('#fullName', 2, 'Vui lòng nhập đầy đủ họ tên'),
			//Validator.isRequired('#email', 'Vui lòng nhập email'),
			//Validator.isEmail('#email'),
			
			Validator.isRequired('#Password', 'Vui lòng nhập mật khẩu'),
			Validator.minLength('#Password', 8),
			Validator.minCharAlpha('#Password', 1),
			Validator.minCharAlphaUpcase('#Password', 1),
			Validator.minNumber('#Password'),
			Validator.minCharSpecial('#Password')
			//Validator.isRequired('input[name=gender]'),
			//Validator.isRequired('input[name=favorite]'),
			//Validator.isRequired('#province'),
			//Validator.isRequired('#avatar'),
		]/*,
		onSubmit: data => {
			
		}*/
	})

	/*if(avatarInput) {
		avatarInput.addEventListener('change', (e) => {
			if(avatarInput.value) {
				const file = e.target.files[0]
				const reader = new FileReader()
				reader.addEventListener('load', (e) => {
					const srcData = e.target.result
					if(srcData) {
						avatarImage.style.backgroundImage = `url('${srcData}')`
						avatarImage.classList.remove('hidden')
						const dataUser = JSON.parse(localStorage.getItem('dataUser')) || {}
						dataUser.srcAvatar = srcData
						localStorage.setItem('dataUser', JSON.stringify(dataUser))
					}
				})
				reader.readAsDataURL(file)
			}
			else {
				avatarImage.classList.add('hidden')
			}
		})
	}*/

	/*if(linkElement) {
		if(linkElement.classList.contains('link-register'))
			linkElement.href = DOMAIN_NAME + 'index.html'
		if(linkElement.classList.contains('link-login'))
			linkElement.href = DOMAIN_NAME + 'assets/pages/login.html'
	}*/
})