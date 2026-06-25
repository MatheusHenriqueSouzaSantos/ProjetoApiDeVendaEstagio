using ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos.ItemVendaInputDtos;
using ApiEstagioBicicletaria.Dtos.VendaDtos.ServicoVendaDtos;
using System.ComponentModel.DataAnnotations;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.VendaInputsDtos
{
    public class VendaUpdateDto
    {
        public Guid? IdCliente { get; set; }
        [Range(0, 1000000, ErrorMessage = "O valor do desconto não pode ser negativo")]
        public decimal? DescontoSobreTotalVenda { get; set; }

        public Guid? IdVendedor { get;  set; }

        public List<Guid>? IdsItensDeletados { get; set; }

        public List<Guid>? IdsServicosDeletados { get; set; }

        public List<ItemVendaCreateDto>? ItensVendaNovos { get; set; }

        public List<ServicoVendaCreateDto>? ServicosVendaNovos { get; set; }

        public List<ItemVendaUpdateDto>? ItensVendaAtualizados { get; set; }
        public List<ServicoVendaUpdateDto>? ServicosVendaAtualizados { get; set; }

        public VendaUpdateDto()
        {
        }

        public VendaUpdateDto(Guid? idCliente, decimal? descontoSobreTotalVenda, Guid? idVendedor, List<Guid>? idsItensDeletados, List<Guid>? idsServicosDeletados,
            List<ItemVendaCreateDto>? itensVendaNovos, List<ServicoVendaCreateDto>? servicosVendaNovos, List<ItemVendaUpdateDto>? itensVendaAtualizados,
            List<ServicoVendaUpdateDto>? servicosVendaAtualizados)
        {
            IdCliente = idCliente;
            DescontoSobreTotalVenda = descontoSobreTotalVenda;
            IdVendedor = idVendedor;
            IdsItensDeletados = idsItensDeletados;
            IdsServicosDeletados = idsServicosDeletados;
            ItensVendaNovos = itensVendaNovos;
            ServicosVendaNovos = servicosVendaNovos;
            ItensVendaAtualizados = itensVendaAtualizados;
            ServicosVendaAtualizados = servicosVendaAtualizados;
        }
    }
}
