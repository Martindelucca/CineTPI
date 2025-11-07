// Esperamos a que todo el HTML esté cargado
// Esperamos a que todo el HTML esté cargado
document.addEventListener("DOMContentLoaded", function() {

    // 1. Obtenemos el formulario
    const loginForm = document.getElementById("login-form");
    const errorMessage = document.getElementById("error-message");

    // 2. Añadimos un "escuchador" para el evento 'submit'
    loginForm.addEventListener("submit", async function(event) {

        // Prevenimos que el formulario se envíe de la forma tradicional
        event.preventDefault(); 

        // Limpiamos errores previos
        errorMessage.textContent = "";

        // 3. Obtenemos los valores de los inputs
        const nroDoc = document.getElementById("nroDoc").value;
        const password = document.getElementById("password").value;

        // 4. Body idéntico al JSON de Swagger
        const loginData = {
            nroDoc: nroDoc,
            password: password
        };

        console.log('Datos que se están enviando a la API:', loginData);

        try {
            const response = await fetch('/api/auth/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(loginData)
            });

            if (response.ok) {
                const data = await response.json();

                // Guardamos token e info mínima
                localStorage.setItem('token', data.token);
                localStorage.setItem('usuarioNombre', data.nombreCompleto || 'Admin');

                // Redirigimos al home/cartelera
                window.location.href = 'index.html';

            } else {
                const errorData = await response.text();
                errorMessage.textContent = errorData || 'Usuario o contraseña incorrectos.';
            }

        } catch (error) {
            console.error('Error de red:', error);
            errorMessage.textContent = 'Error de conexión con el servidor. Intente más tarde.';
        }
    });
});
