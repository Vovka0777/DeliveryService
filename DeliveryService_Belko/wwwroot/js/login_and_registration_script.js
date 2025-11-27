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

        // === НОВОЕ: Сброс состояния на "Вход" при каждом открытии ===
        const formBox = document.querySelector('.form-box');
        const block = document.querySelector('.block');
        const blockContainer = document.querySelector('.block-container');

        // Убираем класс active, который отвечает за показ регистрации
        if (formBox) formBox.classList.remove('active');
        if (block) block.classList.remove('active');
        if (blockContainer) blockContainer.classList.add('signin-active');
        // ============================================================

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

    // === Переключение между Вход / Регистрация (Десктоп) ===
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

    // === Логика для МОБИЛЬНЫХ (Переключатели внутри формы) ===
    const mobileSignUpBtn = document.getElementById('mobile-signup-btn');
    const mobileSignInBtn = document.getElementById('mobile-signin-btn');

    if (mobileSignUpBtn && formBox) {
        mobileSignUpBtn.addEventListener('click', function (e) {
            e.preventDefault();
            formBox.classList.add('active');
            if (block) block.classList.add('active');
        });
    }

    if (mobileSignInBtn && formBox) {
        mobileSignInBtn.addEventListener('click', function (e) {
            e.preventDefault();
            formBox.classList.remove('active');
            if (block) block.classList.remove('active');
        });
    }

    // === AJAX Вход ===
    const form_btn_signin = document.querySelector('.form-btn');
    const errorContainerSignIn = document.getElementById('error-messages-signin');

    if (form_btn_signin) {
        form_btn_signin.addEventListener('click', function (e) {
            e.preventDefault();

            // Ищем по ID (тип text)
            const loginInput = document.getElementById('loginInput');
            const passInput = document.getElementById('passwordInput');

            if (!loginInput || !passInput) return;

            const requestURL = '/Home/Login';

            const body = {
                login: loginInput.value,
                password: passInput.value
            };

            const originalText = form_btn_signin.textContent;
            form_btn_signin.textContent = "Вход...";
            form_btn_signin.disabled = true;

            sendRequest('POST', requestURL, body)
                .then(data => {
                    console.log('Успешный вход:', data);
                    window.location.href = '/';
                })
                .catch(err => {
                    console.error('Ошибка входа:', err);
                    form_btn_signin.textContent = originalText;
                    form_btn_signin.disabled = false;

                    const formBox = document.querySelector('.form-box');
                    formBox.classList.add('shake');
                    setTimeout(() => formBox.classList.remove('shake'), 500);

                    let errors = [];
                    if (err.description) errors.push(err.description);
                    else if (Array.isArray(err)) errors = err;
                    else if (typeof err === 'string') errors.push(err);
                    else errors.push("Неверный логин или пароль");

                    displayErrors(errors, errorContainerSignIn);
                });
        });
    }

    // === AJAX Регистрация ===
    const form_btn_signup = document.querySelector('.form_btn_signup');
    const errorContainerSignUp = document.getElementById('error-messages-signup');

    if (form_btn_signup) {
        form_btn_signup.addEventListener('click', function (e) {
            e.preventDefault();

            const loginInput = document.querySelector('.form_signup input[type="text"]');
            const emailInput = document.querySelector('.form_signup input[type="email"]');
            const passInputs = document.querySelectorAll('.form_signup input[type="password"]');
            const passInput = passInputs[0];
            const passConfInput = passInputs[1];

            const requestURL = '/Home/Register';
            const confirmURL = '/Home/ConfirmEmail';

            const body = {
                login: loginInput?.value,
                email: emailInput?.value,
                password: passInput?.value,
                passwordConfirm: passConfInput?.value,
            };

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

                    const userCode = prompt("Код подтверждения отправлен на почту " + body.email + ". Введите код:");
                    if (!userCode) throw ["Отмена подтверждения"];

                    const confirmBody = {
                        email: body.email,
                        login: body.login,
                        password: body.password,
                        code: userCode,
                        confirmCode: data.data
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
            div.className = 'error';
            div.textContent = error;
            container.appendChild(div);
        });
    }

    // Google Auth Buttons
    const googleBtns = document.querySelectorAll('.google');
    googleBtns.forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            window.location.href = `/Home/AuthenticationGoogle?ReturnUrl=${encodeURIComponent(window.location.href)}`;
        });
    });
});

// Добавляем CSS анимацию "тряски" программно
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