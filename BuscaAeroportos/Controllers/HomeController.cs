using BuscaAeroportos.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using BuscaAeroportos.Geocoding;
using MongoDB.Driver.GeoJsonObjectModel;
using Utilidades.Extensions;

namespace BuscaAeroportos.Controllers
{
	public class HomeController : Controller
	{
		#region Private Methods

		private Coordenada ObterCoordenadasDaLocalizacao(string endereco)
		{
			string url = $"http://maps.google.com/maps/api/geocode/json?address={endereco}";
			Debug.WriteLine(url);

			var coord = new Coordenada("Não Localizado", "-10", "-10");
			var response = new WebClient().DownloadString(url);
			var googleGeocode = JsonConvert.DeserializeObject<GoogleGeocodeResponse>(response);
			Debug.WriteLine(googleGeocode);

			if (googleGeocode.status == "OK")
			{
				coord.Nome = googleGeocode.results[0].formatted_address;
				coord.Latitude = googleGeocode.results[0].geometry.location.lat;
				coord.Longitude = googleGeocode.results[0].geometry.location.lng;
			}

			return coord;
		}

		#endregion Private Methods

		#region Public Methods

		// GET: Home
		public ActionResult Index()
		{
			//coordenadas quaisquer para mostrar o mapa
			var coordenadas = new Coordenada("São Paulo", "-23.64340873969638", "-46.730219057147224");
			return View(coordenadas);
		}

		public async Task<JsonResult> Localizar(HomeViewModel model)
		{
			Debug.WriteLine(model);

			//Captura a posição atual e adiciona a lista de pontos
			Coordenada coordLocal = ObterCoordenadasDaLocalizacao(model.Endereco);
			var aeroportosProximos = new List<Coordenada>();
			aeroportosProximos.Add(coordLocal);

			//Captura a latitude e longitude locais (replace para arrumar a pontuacao dos numeros)
			double lat = coordLocal.Latitude.Replace(".", ",").ToDouble();
			double lon = coordLocal.Longitude.Replace(".", ",").ToDouble();

			//Testa o tipo de aeroporto que será usado na consulta
			var tipoAero = "";

			if (model.Tipo == TipoAeroporto.Internacionais)
				tipoAero = "International";
			else if (model.Tipo == TipoAeroporto.Municipais)
				tipoAero = "Municipal";

			//Captura o valor da distancia
			int distancia = model.Distancia * 1000;

			//Conecta MongoDB
			var conexao = new conectandoMongoDB();

			//Configura o ponto atual no mapa
			var ponto = new GeoJson2DGeographicCoordinates(-118.325258, 34.103212);
			var localizacao = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(ponto);

			// filtro

			//Captura  a lista

			//Escreve os pontos

			return Json(aeroportosProximos);
		}

		#endregion Public Methods
	}
}