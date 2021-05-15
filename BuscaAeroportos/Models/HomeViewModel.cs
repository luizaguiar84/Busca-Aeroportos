using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuscaAeroportos.Models
{
    public enum TipoAeroporto
    {
        Todos,
        Municipais,
        Internacionais
    }

    public class HomeViewModel
    {
        [Required]
        public string Endereco { get; set; }
        [Display(Name ="Distância em Km")]
        public int Distancia { get; set; }
        public TipoAeroporto Tipo { get; set; }

        public override string ToString()
        {
            return $"Endereço: {Endereco}, Distância: {Distancia} Km, Tipo: {Tipo}.";
        }
    }
}