document.addEventListener("DOMContentLoaded", function () {

    obtenerProductos();

    document.getElementById("formCrearProducto")
        .addEventListener("submit", guardarProducto);

    document.getElementById("formEditarProducto")
        .addEventListener("submit", guardarProducto);

    document.getElementById("tblProductos")
        .addEventListener("click", manejarAccion);
});

async function obtenerProductos() {

    const modulo = document.getElementById("moduloProductos");
    const cuerpo = document.querySelector("#tblProductos tbody");

    try {
        const respuesta = await fetch(modulo.dataset.listar);

        if (!respuesta.ok) {
            throw new Error("No se pudieron consultar los productos.");
        }

        const productos = await respuesta.json();

        cuerpo.replaceChildren();

        if (productos.length === 0) {
            mostrarFila("No hay productos registrados.");
            return;
        }

        productos.forEach(function (producto) {

            const fila = document.createElement("tr");

            agregarCelda(fila, producto.id);
            agregarCelda(fila, producto.nombre);
            agregarCelda(fila, producto.descripcion);
            agregarCelda(fila, producto.precio);
            agregarCelda(fila, producto.stock);
            agregarCelda(fila, producto.categoriaNombre);

            const acciones = document.createElement("td");

            const editar = document.createElement("button");
            editar.type = "button";
            editar.className = "btn btn-warning btn-sm me-2";
            editar.textContent = "Editar";
            editar.dataset.accion = "editar";
            editar.dataset.id = producto.id;

            const eliminar = document.createElement("button");
            eliminar.type = "button";
            eliminar.className = "btn btn-danger btn-sm";
            eliminar.textContent = "Eliminar";
            eliminar.dataset.accion = "eliminar";
            eliminar.dataset.id = producto.id;

            acciones.appendChild(editar);
            acciones.appendChild(eliminar);

            fila.appendChild(acciones);
            cuerpo.appendChild(fila);

        });

    } catch (error) {
        console.error(error);
        mostrarFila("Error al cargar los productos.");
    }
}

function agregarCelda(fila, valor) {

    const celda = document.createElement("td");
    celda.textContent = valor ?? "";
    fila.appendChild(celda);
}

function mostrarFila(mensaje) {

    const cuerpo = document.querySelector("#tblProductos tbody");
    cuerpo.replaceChildren();

    const fila = document.createElement("tr");
    const celda = document.createElement("td");

    celda.colSpan = 7;
    celda.className = "text-center";
    celda.textContent = mensaje;

    fila.appendChild(celda);
    cuerpo.appendChild(fila);
}

async function guardarProducto(evento) {

    evento.preventDefault();

    const formulario = evento.currentTarget;
    const esEdicion = formulario.id === "formEditarProducto";

    const mensaje = document.getElementById(
        esEdicion ? "mensajeEditarProducto" : "mensajeCrearProducto"
    );

    mensaje.classList.add("d-none");

    try {
        const respuesta = await fetch(formulario.action, {
            method: "POST",
            body: new FormData(formulario)
        });

        const resultado = await respuesta.json();

        if (!respuesta.ok || !resultado.esCorrecto) {
            mostrarError(
                mensaje,
                resultado.mensaje ||
                "No se pudo guardar el producto."
            );
            return;
        }

        alert(resultado.mensaje);

        const idModal = esEdicion
            ? "modalEditarProducto"
            : "modalCrearProducto";

        bootstrap.Modal.getOrCreateInstance(
            document.getElementById(idModal)
        ).hide();

        formulario.reset();

        await obtenerProductos();

    } catch (error) {
        console.error(error);
        mostrarError(mensaje, "Ocurrió un error al guardar.");
    }
}

async function manejarAccion(evento) {

    const boton = evento.target.closest("button[data-accion]");

    if (!boton) {
        return;
    }

    if (boton.dataset.accion === "editar") {
        await editarProducto(boton.dataset.id);
    }

    if (boton.dataset.accion === "eliminar") {
        await eliminarProducto(boton.dataset.id);
    }
}

async function editarProducto(id) {

    const modulo = document.getElementById("moduloProductos");
    const formulario = document.getElementById("formEditarProducto");

    try {
        const respuesta = await fetch(
            modulo.dataset.detalle + "?id=" + encodeURIComponent(id)
        );

        if (!respuesta.ok) {
            throw new Error("Producto no encontrado.");
        }

        const producto = await respuesta.json();

        formulario.elements.namedItem("Id").value =
            producto.id;

        formulario.elements.namedItem("Nombre").value =
            producto.nombre;

        formulario.elements.namedItem("Descripcion").value =
            producto.descripcion ?? "";

        formulario.elements.namedItem("Precio").value =
            producto.precio;

        formulario.elements.namedItem("Stock").value =
            producto.stock;

        formulario.elements.namedItem("CategoriaId").value =
            producto.categoriaId;

        bootstrap.Modal.getOrCreateInstance(
            document.getElementById("modalEditarProducto")
        ).show();

    } catch (error) {
        console.error(error);
        alert("No se pudo cargar el producto.");
    }
}

async function eliminarProducto(id) {

    if (!confirm("¿Desea eliminar este producto?")) {
        return;
    }

    const formulario = document.getElementById("formEliminarProducto");

    formulario.elements.namedItem("id").value = id;

    try {
        const respuesta = await fetch(formulario.action, {
            method: "POST",
            body: new FormData(formulario)
        });

        const resultado = await respuesta.json();

        alert(resultado.mensaje || "No se pudo eliminar.");

        if (respuesta.ok && resultado.esCorrecto) {
            await obtenerProductos();
        }

    } catch (error) {
        console.error(error);
        alert("Ocurrió un error al eliminar.");
    }
}

function mostrarError(elemento, mensaje) {

    elemento.textContent = mensaje;
    elemento.classList.remove("d-none");
}
