using System;
using System.Collections.Generic;
using System.Linq;
using Xadrez.Models.Pecas;

namespace Xadrez.Models
{
    public class Tabuleiro : ITabuleiro
    {
        public List<Casa> Casas { get; }
        public List<IPeca> Pecas { get; private set; }
        public List<IPeca> PecasCapturadas { get; private set; }

        public IEnumerable<IPeca> PecasBrancas => Pecas.Where(p => p.EBranca);
        public IEnumerable<IPeca> PecasPretas => Pecas.Where(p => !p.EBranca);
        public IEnumerable<IPeca> PecasBrancasCapturadas => PecasCapturadas.Where(p => p.EBranca);
        public IEnumerable<IPeca> PecasPretasCapturadas => PecasCapturadas.Where(p => !p.EBranca);

        public Tabuleiro()
        {
            Casas = Enumerable.Range(0, 8)
                .SelectMany(linha => Enumerable.Range(0, 8)
                    .Select(coluna => new Casa(linha, coluna)))
                .ToList();

            Pecas = new List<IPeca>();
            PecasCapturadas = new List<IPeca>();
        }

        public void DistribuiPecas()
        {
            Pecas.Clear();
            PecasCapturadas.Clear();
            Casas.ForEach(c => c.Peca = null);

            void AdicionaLinhaInicial(int linha, bool eBranca)
            {
                AdicionaPeca(new Torre(eBranca), linha, 0);
                AdicionaPeca(new Cavalo(eBranca), linha, 1);
                AdicionaPeca(new Bispo(eBranca), linha, 2);
                AdicionaPeca(new Rainha(eBranca), linha, 3);
                AdicionaPeca(new Rei(eBranca), linha, 4);
                AdicionaPeca(new Bispo(eBranca), linha, 5);
                AdicionaPeca(new Cavalo(eBranca), linha, 6);
                AdicionaPeca(new Torre(eBranca), linha, 7);
            }

            AdicionaLinhaInicial(0, true);
            for (int i = 0; i < 8; i++) AdicionaPeca(new Peao(true), 1, i);

            AdicionaLinhaInicial(7, false);
            for (int i = 0; i < 8; i++) AdicionaPeca(new Peao(false), 6, i);
        }

        public bool ValidaMovimento(Jogador jogador, Movimento movimento)
        {
            if (movimento.Peca.EBranca != jogador.EBranco)
                return false;

            if (!movimento.Peca.MovimentosPossiveis(this).Any(m => m.Equals(movimento)))
                return false;

            ExecutaMovimento(movimento);
            bool emXeque = VerificaXeque(jogador.EBranco);
            ReverteMovimento(movimento);

            return !emXeque;
        }

        public void ExecutaMovimento(Movimento movimento)
        {
            if (movimento.PecaCapturada != null)
            {
                Pecas.Remove(movimento.PecaCapturada);
                PecasCapturadas.Add(movimento.PecaCapturada);
            }

            movimento.CasaOrigem.Peca = null;
            movimento.CasaDestino.Peca = movimento.Peca;
            movimento.Peca.FoiMovimentada = true;
        }

        public void ReverteMovimento(Movimento movimento)
        {
            movimento.CasaOrigem.Peca = movimento.Peca;
            movimento.CasaDestino.Peca = movimento.PecaCapturada;

            if (movimento.PecaCapturada != null)
            {
                PecasCapturadas.Remove(movimento.PecaCapturada);
                Pecas.Add(movimento.PecaCapturada);
            }

            movimento.Peca.FoiMovimentada = false;
        }

        public Casa? ObtemCasaPeca(IPeca peca) =>
            Casas.FirstOrDefault(c => c.Peca == peca);

        private void AdicionaPeca(IPeca peca, int linha, int coluna)
        {
            var casa = Casas.FirstOrDefault(c => c.Linha == linha && c.Coluna == coluna);
            if (casa != null)
            {
                casa.Peca = peca;
                Pecas.Add(peca);
            }
        }

        public bool VerificaXeque(bool eBranca)
        {
            var rei = Pecas.OfType<IRei>().FirstOrDefault(p => p.EBranca == eBranca);
            var casaRei = rei == null ? null : ObtemCasaPeca(rei);
            if (rei == null || casaRei == null) return false;

            return Pecas.Where(p => p.EBranca != eBranca)
                        .Any(p => p.MovimentosPossiveis(this)
                                   .Any(m => m.CasaDestino == casaRei));
        }

        public bool VerificaXequeMate(bool eBranca)
        {
            if (!VerificaXeque(eBranca)) return false;

            return !Pecas.Where(p => p.EBranca == eBranca)
                         .Any(p =>
                         {
                             var casaOrigem = ObtemCasaPeca(p);
                             return casaOrigem != null &&
                                    p.MovimentosPossiveis(this)
                                     .Any(m =>
                                     {
                                         ExecutaMovimento(m);
                                         bool aindaEmXeque = VerificaXeque(eBranca);
                                         ReverteMovimento(m);
                                         return !aindaEmXeque;
                                     });
                         });
        }

        public bool VerificaPerigo(Casa casa, bool eBranca)
        {
            return Pecas.Where(p => p.EBranca != eBranca)
                        .Any(p => p.MovimentosPossiveis(this)
                                   .Any(m => m.CasaDestino == casa));
        }
    }
}
