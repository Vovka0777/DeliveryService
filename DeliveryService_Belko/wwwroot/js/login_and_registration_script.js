document.addEventListener('DOMContentLoaded', function () {
    function hiddenOpen_Closeclick() {
        let x = document.querySelector(".container-login-registration");
        if (x.style.display == "none") {
            x.style.display = "grid";
        } else {
            x.style.display = "none";
        }
    }

    const clickHide = document.getElementById("click-to-hide");
    if (clickHide) clickHide.addEventListener("click", hiddenOpen_Closeclick);
    const overlay = document.querySelector(".overlay");
    if (overlay) overlay.addEventListener("click", hiddenOpen_Closeclick);

    const signInBtn = document.querySelector('.signin-btn');
    const signUpBtn = document.querySelector('.signup-btn');
    const formBox = document.querySelector('.form-box');
    const block = document.querySelector('.block');
    const blockContainer = document.querySelector('.block-container'); // <-- Добавляем эту строку

    if (signInBtn && signUpBtn) {
        signUpBtn.addEventListener('click', function () {
            formBox.classList.add('active');
            block.classList.add('active');
            blockContainer.classList.remove('signin-active'); // <-- Удаляем класс для Входа
        });

        signInBtn.addEventListener('click', function () {
            formBox.classList.remove('active');
            block.classList.remove('active');
            blockContainer.classList.add('signin-active'); // <-- Добавляем класс для Входа
        });
    }

const modal = document.getElementById('loginModal');

modal?.addEventListener('click', (event) => {
    if (event.target === modal) {
        modal.style.display = 'none';
    }
});

const form_btn_signin = document.querySelector('.form-btn');
const form_btn_signup = document.querySelector('.form_btn_signup');

if (form_btn_signin) {
    form_btn_signin.addEventListener('click', function () {
        const requestURL = '/Home/Login';  // Должен быть правильный URL для логина


        const form = {
            email: document.querySelector('.form_signin #email input'),
            password: document.querySelector('.form_signin #password input')
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
                window.location.href = '/';
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

    form_btn_signup.addEventListener('click', function () {
        const requestURL = '/Home/Register';
        const confirmURL = '/Home/ConfirmEmail'; // ⬅️ URL для подтверждения
        const errorContainer = document.getElementById('error-messages-signup');

        const form = {
            login: document.querySelector('.form_signup #login input'),
            email: document.querySelector('.form_signup #email input'),
            password: document.querySelector('.form_signup #password input'),
            passwordConfirm: document.querySelector('.form_signup #confirm_password input')
        };

        // Собираем данные для первого этапа
        const body = {
            login: form.login.value,
            email: form.email.value,
            password: form.password.value,
            passwordConfirm: form.passwordConfirm.value,
        };

        // 1. Отправляем запрос на регистрацию (отправка письма)
        sendRequest('POST', requestURL, body)
            .then(data => {
                console.log('Письмо отправлено, код получен (скрыто):', data);

                // Очищаем ошибки, если были
                errorContainer.innerHTML = '';

                // 2. Просим пользователя ввести код
                // Для простоты пока используем prompt. В будущем можно сделать красивое модальное окно.
                const userCode = prompt("На ваш Email отправлен код подтверждения. Введите его сюда:");

                if (!userCode) {
                    throw ["Вы не ввели код подтверждения. Регистрация отменена."];
                }

                // 3. Формируем данные для ConfirmEmailViewModel
                // Обратите внимание: data.data - это код, который вернул сервер в методе Register
                const confirmBody = {
                    email: body.email,
                    login: body.login,
                    password: body.password,
                    code: userCode,          // Код, который ввел пользователь
                    confirmCode: data.data   // Оригинальный код от сервера
                };

                // 4. Отправляем запрос на подтверждение и сохранение
                return sendRequest('POST', confirmURL, confirmBody);
            })
            .then(finalResponse => {
                // 5. Если всё прошло успешно
                console.log('Регистрация завершена:', finalResponse);
                alert("Регистрация прошла успешно! Вы вошли в систему.");

                cleaningAndClosingForm(form, errorContainer);
                window.location.href = '/'; // Переадресация на главную (уже авторизованным)
            })
            .catch(err => {
                console.log('Ошибка:', err);
                // Проверяем формат ошибки, так как она может прийти с разных этапов
                if (Array.isArray(err)) {
                    displayErrors(err, errorContainer);
                } else if (err.description) {
                    displayErrors([err.description], errorContainer);
                } else if (err.errors && Array.isArray(err.errors)) {
                    displayErrors(err.errors, errorContainer);
                } else {
                    displayErrors(['Произошла ошибка при регистрации'], errorContainer);
                }
            });
    });
    const sideMenuButton = document.getElementById("side-menu-button-click-to-hide");
    if (sideMenuButton) sideMenuButton.addEventListener("click", hiddenOpen_Closeclick);

    const google = document.querySelectorAll('.google');

    if (google) {
        google.forEach(btn => {
            btn.addEventListener('click', function () {
                window.location.href = `/Home/AuthenticationGoogle?ReturnUrl=${encodeURIComponent(window.location.href)}`;
            });
        });
    }
});