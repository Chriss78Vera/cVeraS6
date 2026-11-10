using cVeraS6.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace cVeraS6.Views;

public partial class vEstudiante : ContentPage
{
    public const string Url = "http://10.2.14.146/vw_uisrael/post.php";
    private readonly HttpClient client = new();
    private readonly ObservableCollection<Estudiante> _estudiantes = [];

    public vEstudiante()
    {
        InitializeComponent();
        coleccionEstudiantes.ItemsSource = _estudiantes;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await MostrarEstudiantes();
    }

    private async Task MostrarEstudiantes()
    {
        try
        {
            var content = await client.GetStringAsync(Url);
            var estudiantes = JsonConvert.DeserializeObject<ObservableCollection<Estudiante>>(content) ?? [];

            _estudiantes.Clear();
            foreach (var estudiante in estudiantes)
            {
                _estudiantes.Add(estudiante);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"No se pudieron cargar los estudiantes: {ex.Message}", "Aceptar");
        }
    }
}
