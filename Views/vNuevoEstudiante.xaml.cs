using cVeraS6.Models;

namespace cVeraS6.Views;

public partial class vNuevoEstudiante : ContentPage
{
    public const string Url = "http://10.2.4.16/vw_uisrael/post.php";
    private readonly HttpClient client = new();

    public vNuevoEstudiante()
    {
        InitializeComponent();
    }

    private async void btnAgregar_Clicked(object sender, EventArgs e)
    {
        var nombre = txtNombre.Text?.Trim() ?? string.Empty;
        var apellido = txtApellido.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido))
        {
            await DisplayAlertAsync("Validación", "Ingrese el nombre y el apellido.", "Aceptar");
            return;
        }

        if (!int.TryParse(txtEdad.Text, out var edad) || edad <= 0 || edad > 120)
        {
            await DisplayAlertAsync("Validación", "Ingrese una edad válida entre 1 y 120.", "Aceptar");
            return;
        }

        var estudiante = new Estudiante
        {
            nombre = nombre,
            apellido = apellido,
            edad = edad
        };

        try
        {
            var contenido = new MultipartFormDataContent();
            contenido.Add(new StringContent(estudiante.nombre), "nombre");
            contenido.Add(new StringContent(estudiante.apellido), "apellido");
            contenido.Add(new StringContent(estudiante.edad.ToString()), "edad");

            var respuesta = await client.PostAsync(Url, contenido);

            if (respuesta.IsSuccessStatusCode)
            {
                await DisplayAlertAsync("Correcto", "El estudiante fue agregado.", "Aceptar");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlertAsync("Error", "No se pudo agregar el estudiante.", "Aceptar");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "Aceptar");
        }
    }
}
