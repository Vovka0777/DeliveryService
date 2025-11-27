document.addEventListener('DOMContentLoaded', function () {
    // === Логика открытия/закрытия модального окна ===
    function toggleModal() {
        const container = document.querySelector(".container-login-registration");
        if (!container) return;

        const isHidden = container.style.display === "none" || container.style.display === "";

        if (isHidden) {
            container.style.display = "flex"; // Flex для центрирования
            // Блокируем скролл страницы
            document.body.style.overflow = "hidden";
        } else {
            container.style.display = "none";
            document.body.style.overflow = "";
        }
    }

    const clickShowBtns = document.querySelectorAll("#click-to-hide, #open-login-side");
    clickShowBtns.forEach(btn => btn.addEventListener("click", (e) => {
        e.preventDefault();
        toggleModal();
    }));

    const overlay = document.querySelector(".container-login-registration");
    // Закрытие при клике на темный фон
    if (overlay) {
        overlay.addEventListener("click", function (e) {
            if (e.target === overlay) {
                toggleModal();
            }
        });
    }

    // === Переключение между Вход / Регистрация ===
    const signInBtn = document.querySelector('.signin-btn');
    const signUpBtn = document.querySelector('.signup-btn');
    const formBox = document.querySelector('.form-box');
    const block = document.querySelector('.block');
    const blockContainer = document.querySelector('.block-container');

    if (signInBtn && signUpBtn && formBox) {
        signUpBtn.addEventListener('click', function () {
            formBox.classList.add('active');
            block.classList.add('active');
            if (blockContainer) blockContainer.classList.remove('signin-active');
        });

        signInBtn.addEventListener('click', function () {
            formBox.classList.remove('active');
            block.classList.remove('active');
            if (blockContainer) blockContainer.classList.add('signin-active');
        });
    }

    // === AJAX Вход ===
    const form_btn_signin = document.querySelector('.form-btn'); // Кнопка "Войти" внутри формы
    const errorContainerSignIn = document.getElementById('error-messages-signin');

    if (form_btn_signin) {
        form_btn_signin.addEventListener('click', function (e) {
            e.preventDefault(); // Важно предотвратить стандартный сабмит

            const emailInput = document.querySelector('.form_signin input[type="email"]');
            const passInput = document.querySelector('.form_signin input[type="password"]');

            if (!emailInput || !passInput) return;

            const requestURL = '/Home/Login';
            const body = {
                email: emailInput.value,
                password: passInput.value
            };

            // Визуальная индикация загрузки
            const originalText = form_btn_signin.textContent;
            form_btn_signin.textContent = "Вход...";
            form_btn_signin.disabled = true;

            sendRequest('POST', requestURL, body)
                .then(data => {
                    console.log('Успешный вход:', data);
                    // Перезагрузка
                    window.location.href = '/';
                })
                .catch(err => {
                    console.error('Ошибка входа:', err);
                    form_btn_signin.textContent = originalText;
                    form_btn_signin.disabled = false;

                    // Анимация ошибки
                    const formBox = document.querySelector('.form-box');
                    formBox.classList.add('shake');
                    setTimeout(() => formBox.classList.remove('shake'), 500);

                    // Отображение текста ошибки
                    let errors = [];
                    if (err.description) errors.push(err.description);
                    else if (Array.isArray(err)) errors = err;
                    else errors.push("Неверный логин или пароль");

                    displayErrors(errors, errorContainerSignIn);
                });
        });
    }

    // === AJAX Регистрация ===
    const form_btn_signup = document.querySelector('.form_btn_signup'); // Кнопка "Зарегистрироваться"
    const errorContainerSignUp = document.getElementById('error-messages-signup'); // Нужно добавить этот ID в HTML, если его нет

    if (form_btn_signup) {
        form_btn_signup.addEventListener('click', function (e) {
            e.preventDefault();

            const loginInput = document.querySelector('.form_signup input[type="text"]');
            const emailInput = document.querySelector('.form_signup input[type="email"]');
            const passInput = document.querySelector('.form_signup input[type="password"]');
            const passConfInput = document.querySelectorAll('.form_signup input[type="password"]')[1];

            const requestURL = '/Home/Register';
            const confirmURL = '/Home/ConfirmEmail';

            const body = {
                login: loginInput?.value,
                email: emailInput?.value,
                password: passInput?.value,
                passwordConfirm: passConfInput?.value,
            };

            // Валидация на клиенте (минимум)
            if (body.password !== body.passwordConfirm) {
                displayErrors(["Пароли не совпадают"], errorContainerSignUp);
                return;
            }

            form_btn_signup.textContent = "Отправка...";
            form_btn_signup.disabled = true;

            sendRequest('POST', requestURL, body)
                .then(data => {
                    form_btn_signup.textContent = "Зарегистрироваться";
                    form_btn_signup.disabled = false;

                    // Код отправлен
                    const userCode = prompt("Код подтверждения отправлен на почту " + body.email + ". Введите код:");
                    if (!userCode) throw ["Отмена подтверждения"];

                    const confirmBody = {
                        email: body.email,
                        login: body.login,
                        password: body.password,
                        code: userCode,
                        confirmCode: data.data // Код от сервера
                    };

                    return sendRequest('POST', confirmURL, confirmBody);
                })
                .then(finalResponse => {
                    alert("Регистрация успешна! Добро пожаловать.");
                    window.location.href = '/';
                })
                .catch(err => {
                    form_btn_signup.textContent = "Зарегистрироваться";
                    form_btn_signup.disabled = false;

                    let errors = [];
                    if (Array.isArray(err)) errors = err;
                    else if (err.description) errors.push(err.description);
                    else if (err.errors) errors = Object.values(err.errors).flat();
                    else errors.push("Ошибка регистрации");

                    if (errorContainerSignUp) displayErrors(errors, errorContainerSignUp);
                    else alert(errors.join('\n'));
                });
        });
    }

    // --- Helpers ---
    function sendRequest(method, url, body = null) {
        const headers = { 'Content-Type': 'application/json' };
        return fetch(url, {
            method: method,
            body: JSON.stringify(body),
            headers: headers
        }).then(response => {
            if (!response.ok) {
                return response.json().then(errorData => { throw errorData; });
            }
            return response.json();
        });
    }

    function displayErrors(errors, container) {
        if (!container) return;
        container.innerHTML = '';
        errors.forEach(error => {
            const div = document.createElement('div');
            div.className = 'error'; // Стилизован в CSS
            div.textContent = error;
            container.appendChild(div);
        });
    }

    // Google Auth Buttons
    const googleBtns = document.querySelectorAll('.google');
    googleBtns.forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault(); // Если кнопка внутри формы
            window.location.href = `/Home/AuthenticationGoogle?ReturnUrl=${encodeURIComponent(window.location.href)}`;
        });
    });
});

// Добавляем CSS анимацию "тряски" программно, если её нет в CSS
const styleSheet = document.createElement("style");
styleSheet.innerText = `
@keyframes shake {
  0%, 100% { transform: translateX(0); }
  10%, 30%, 50%, 70%, 90% { transform: translateX(-5px); }
  20%, 40%, 60%, 80% { transform: translateX(5px); }
}
.shake {
  animation: shake 0.5s;
}`;
document.head.appendChild(styleSheet);