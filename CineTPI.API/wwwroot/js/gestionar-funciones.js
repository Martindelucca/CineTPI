document.addEventListener("DOMContentLoaded", async () => {

    const token = localStorage.getItem("token");

    if (!token) {
        alert("Debes iniciar sesión como admin.");
        window.location.href = "login.html";
        return;
    }

    const selPelicula = document.getElementById("idPelicula");
    const selSala = document.getElementById("idSala");
    const selHorario = document.getElementById("idHorario");
    const tablaBody = document.getElementById("tbody-funciones");

    //Cargar selects
    await cargarPeliculas();
    await cargarSalas();
    await cargarHorarios();

    // Cargar funciones existentes
    cargarFunciones();

    // Crear función nueva

    document.getElementById("form-funcion").addEventListener("submit", async (e) => {
        e.preventDefault();

        const body = {
            idPelicula: parseInt(selPelicula.value),
            idSala: parseInt(selSala.value),
            fecha: document.getElementById("fecha").value,
            idHorario: parseInt(selHorario.value)
        };

        const resp = await fetch("/api/funciones", {
            method: "POST",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify(body)
        });

        if (resp.ok) {
            alert("Función creada correctamente.");
            cargarFunciones();
        } else {
            const msg = await resp.text();
            alert("Error: " + msg);
        }
    });

    //  Carga de combos

    async function cargarPeliculas() {
        const resp = await fetch("/api/peliculas", {
            headers: { "Authorization": `Bearer ${token}` }
        });
        const data = await resp.json();

        selPelicula.innerHTML = data
            .map(p => `<option value="${p.idPelicula}">${p.titulo}</option>`)
            .join("");
    }

    async function cargarSalas() {
        const resp = await fetch("/api/salas", {
            headers: { "Authorization": `Bearer ${token}` }
        });
        const data = await resp.json();

        selSala.innerHTML = data
            .map(s => `<option value="${s.idSala}">Sala ${s.nroSala}</option>`)
            .join("");
    }

    async function cargarHorarios() {
        const resp = await fetch("/api/horarios", {
            headers: { "Authorization": `Bearer ${token}` }
        });
        const data = await resp.json();

        selHorario.innerHTML = data
            .map(h => `<option value="${h.idHorario}">${h.horario}</option>`)
            .join("");
    }

    //  Cargar tabla de funciones
    async function cargarFunciones() {
        const resp = await fetch("/api/funciones/todas", {
            headers: { "Authorization": `Bearer ${token}` }
        });

        const funciones = await resp.json();

        tablaBody.innerHTML = funciones.map(f => `
            <tr>
                <td>${f.idFuncion}</td>
                <td>${f.tituloPelicula}</td>
                <td>${f.nombreSala}</td>
                <td>${f.fecha}</td>
                <td>${f.horario}</td>
                <td>
                    <button class="btn-danger" onclick="eliminarFuncion(${f.idFuncion})">
                        Eliminar
                    </button>
                </td>
            </tr>
        `).join("");
    }

});

//  Función  para eliminar

async function eliminarFuncion(id) {
    if (!confirm("¿Seguro que querés borrar esta función?")) return;

    const token = localStorage.getItem("token");

    const resp = await fetch(`/api/funciones/${id}`, {
        method: "DELETE",
        headers: {
            "Authorization": `Bearer ${token}`
        }
    });

    if (resp.ok) {
        alert("Función eliminada.");
        location.reload();
    } else {
        alert("No se puede eliminar. Puede tener reservas asociadas.");
    }
}
