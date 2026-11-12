using cVeraS6.Models;

namespace cVeraS6.Views;

public partial class vEditarEstudiante : ContentPage
{
    public const string Url = "http://10.2.4.16/vw_uisrael/post.php";
    private readonly HttpClient client = new();

    public vEditarEstudiante(Estudiante datos)
    {
        InitializeComponent();
        txtCodigo.Text = datos.codigo.ToString();
        txtNombre.Text = datos.nombre.ToString();
        txtApellido.Text = datos.apellido.ToString();
        txtEdad.Text = datos.edad.ToString();
    }

    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {

        try
        {
            string nombre = Uri.EscapeDataString(txtNombre.Text ?? "");
            string apellido = Uri.EscapeDataString(txtApellido.Text ?? "");
            string edad = Uri.EscapeDataString(txtEdad.Text ?? "");
            string codigo = Uri.EscapeDataString(txtCodigo.Text ?? "");

            string urlActualizar = $"{Url}?nombre={nombre}&apellido={apellido}&edad={edad}&codigo={codigo}";
            var respuesta = await client.PutAsync(urlActualizar, null);

            if (respuesta.IsSuccessStatusCode)
            {
                await DisplayAlertAsync("Correcto", "El estudiante fue actualizado.", "Aceptar");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlertAsync("Error", "No se pudo actualizar el estudiante.", "Aceptar");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "Aceptar");
        }
    }

    private async void btnEliminar_Clicked(object sender, EventArgs e)
    {
        if (!int.TryParse(txtCodigo.Text, out var codigo))
        {
            await DisplayAlertAsync("Error", "Código incorrecto.", "Aceptar");
            return;
        }

        bool confirmar = await DisplayAlertAsync(
            "Confirmar",
            "¿Desea eliminar este estudiante?",
            "Sí",
            "No");

        if (!confirmar)
        {
            return;
        }

        try
        {
            string urlEliminar = $"{Url}?codigo={codigo}";

            var contenido = new MultipartFormDataContent();
            contenido.Add(new StringContent(txtNombre.Text ?? ""), "nombre");
            contenido.Add(new StringContent(txtApellido.Text ?? ""), "apellido");
            contenido.Add(new StringContent(txtEdad.Text ?? ""), "edad");

            var solicitud = new HttpRequestMessage(HttpMethod.Delete, urlEliminar);
            solicitud.Content = contenido;

            var respuesta = await client.SendAsync(solicitud);

            if (respuesta.IsSuccessStatusCode)
            {
                await DisplayAlertAsync("Correcto", "El estudiante fue eliminado.", "Aceptar");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlertAsync("Error", "No se pudo eliminar el estudiante.", "Aceptar");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "Aceptar");
        }
    }
}
