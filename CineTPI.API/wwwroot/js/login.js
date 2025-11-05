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

        // 4. Creamos el objeto (el "body") para enviar a la API
        // ¡Debe ser idéntico al JSON que probamos en Swagger!
        const loginData = {
            nroDoc: nroDoc,
            password: password
        };
// --- ¡AGREGÁ ESTA LÍNEA! ---
console.log('Datos que se están enviando a la API:', loginData);
// --- FIN ---
        // 5. ¡Usamos fetch para llamar a nuestra API!
        try {
            const response = await fetch('/api/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(loginData) // Convertimos el objeto a un string JSON
            });

            // 6. Analizamos la respuesta de la API
            if (response.ok) {
                // ¡Éxito! La API devolvió 200 OK
                const data = await response.json(); // Leemos el JSON de respuesta

                // --- ¡¡CRÍTICO!! Guardamos el token en el navegador ---
                // localStorage es un "mini-almacén" del navegador.
                localStorage.setItem('token', data.token);

                // (Opcional: guardar info del usuario)
                localStorage.setItem('usuarioNombre', data.nombreCompleto);

                // Redirigimos al usuario al menú principal
                window.location.href = 'index.html'; 

            } else {
                // ¡Error! La API devolvió 401 (Unauthorized) u otro error
                const errorData = await response.text(); // Leemos el mensaje de error (ej: "Usuario o contraseña incorrectos")
                errorMessage.textContent = errorData;
            }

        } catch (error) {
            // Error de red (ej: la API no está corriendo)
            console.error('Error de red:', error);
            errorMessage.textContent = 'Error de conexión con el servidor. Intente más tarde.';
        }
    });
});