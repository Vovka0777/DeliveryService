document.addEventListener('DOMContentLoaded', function () {
    function hiddenOpen_Closeclick() {
        let x = document.querySelector(".container-login-registration");
        if (x.style.display == "none") {
            x.style.display = "grid";
        } else {
            x.style.display = "none";
        }
    }

    document.getElementById("click-to-hide").addEventListener("click", hiddenOpen_Closeclick);
    document.querySelector(".overlay").addEventListener("click", hiddenOpen_Closeclick);

    const signInBtn = document.querySelector('.signin-btn');
    const signUpBtn = document.querySelector('.signup-btn');
    const formBox = document.querySelector('.form-box');
    const block = document.querySelector('.block');

    if (signInBtn && signUpBtn) {
        signUpBtn.addEventListener('click', function () {
            formBox.classList.add('active');
            block.classList.add('active');
        });

        signInBtn.addEventListener('click', function () {
            formBox.classList.remove('active');
            block.classList.remove('active');
        });
    }
});

const modal = document.getElementById('loginModal');
const block = modal?.querySelector('.block-container');

modal?.addEventListener('click', (event) => {
    if (event.target === modal) {
        modal.style.display = 'none';
    }
});

const form_btn_signin = document.querySelector('_form_btn_signin');
const form_btn_signup = document.querySelector('_form_btn_signup');

if (form_btn_signin) {
    form_btn_signin.addEventListener('click', function () {
        const requestURL = '/Home/Login';  // Должен быть правильный URL для логина

        const errorContainer = document.getElementById('error-messages-signin');

        const form = {
            email: document.querySelector('#signin_email input'),
            password: document.querySelector('#signin_password input')
        };

        const body = {
            email: form.email.value,
            password: form.password.value
        };

        sendRequest('POST', requestURL, body)
            .then(data => {
                // Очистка формы и закрытие ошибок
                cleaningAndClosingForm(form, errorContainer);
                console.log('Успешный ответ:', data);

                // Перезагрузка страницы после успешного ответа
                location.reload();
            })
            .catch(err => {
                // Обработка ошибок
                console.log('Ошибка:', err);
                displayErrors(err, errorContainer);
            });
    });
}

function sendRequest(method, url, body = null) {
    const headers = {
        'Content-Type': 'application/json'
    };

    return fetch(url, {
        method: method,
        body: JSON.stringify(body),
        headers: headers
    }).then(response => {
        if (!response.ok) {
            return response.json().then(errorData => {
                throw errorData;
            });
        }
        return response.json();
    });
}

// Функция для очистки формы и закрытия ошибок
function cleaningAndClosingForm(form, errorContainer) {
    form.email.value = '';
    form.password.value = '';

    // Очистить контейнер ошибок
    errorContainer.innerHTML = '';
}

// Функция для отображения ошибок
function displayErrors(errors, errorContainer) {
    errorContainer.innerHTML = '';  // Очистить контейнер ошибок
    if (Array.isArray(errors)) {
        errors.forEach(error => {
            const errorMessage = document.createElement('div');
            errorMessage.classList.add('error');
            errorMessage.textContent = error;
            errorContainer.appendChild(errorMessage);
        });
    } else {
        const errorMessage = document.createElement('div');
        errorMessage.classList.add('error');
        errorMessage.textContent = 'Произошла ошибка';
        errorContainer.appendChild(errorMessage);
    }
}

const errorContainer = document.getElementById('error-messages-signin');

function cleaningAndClosingForm(form, errorContainer) {
    errorContainer.innerHTML = '';
    for (const key in form) {
        if (form.hasOwnProperty(key)) {
            form[key].value = '';
        }
    }
    hiddenOpen_Closeclick();
}

if (form_btn_signup) {
    form_btn_signup.addEventListener('click', function () {
        const requestURL = '/Home/Regiser';

        const errorContainer = document.getElementById('error-messages-signup');

        const form = {
            login: document.getElementById("signup_login");
            email: document.querySelector('#signin_email input'),
            password: document.querySelector('#signin_password input'),
            passwordConfirm: document.getElementById("signup_confirm_password"),
        };

        const body = {
            login: form.login.value,
            email: form.email.value,
            password: form.password.value,
            passwordConfirm: form.passwordConfirm.value,
        };

        sendRequest('POST', requestURL, body)
            .then(data => {
                cleaningAndClosingForm(form, errorContainer);
                console.log('Успешный ответ:', data);
                location.reload();
            })
            .catch(err => {
                console.log('Ошибка:', err);
                displayErrors(err, errorContainer);
            });
    });
}