using Xadrez.Models.Pecas;
using System.Linq;

namespace Xadrez.Models.Pecas
{
    public class Torre(bool eBranca) : Peca(eBranca), ITorre
    {
        public override List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
        {
            var movimentos = new List<Movimento>();
            var casaAtual = tabuleiro.ObtemCasaPeca(this);
            if (casaAtual is null) return movimentos;

            // Direção: Cima
            for (int i = casaAtual.Linha + 1; i < 8; i++)
            {
                var casa = tabuleiro.Casas.FirstOrDefault(c => c.Linha == i && c.Coluna == casaAtual.Coluna);
                if (casa == null) break;
                if (casa.Peca == null)
                    movimentos.Add(new Movimento(this, casaAtual, casa));
                else
                {
                    if (casa.Peca.EBranca != this.EBranca)
                        movimentos.Add(new Movimento(this, casaAtual, casa, casa.Peca));
                    break;
                }
            }

            // Direção: Baixo
            for (int i = casaAtual.Linha - 1; i >= 0; i--)
            {
                var casa = tabuleiro.Casas.FirstOrDefault(c => c.Linha == i && c.Coluna == casaAtual.Coluna);
                if (casa == null) break;
                if (casa.Peca == null)
                    movimentos.Add(new Movimento(this, casaAtual, casa));
                else
                {
                    if (casa.Peca.EBranca != this.EBranca)
                        movimentos.Add(new Movimento(this, casaAtual, casa, casa.Peca));
                    break;
                }
            }

            // Direção: Direita
            for (int j = casaAtual.Coluna + 1; j < 8; j++)
            {
                var casa = tabuleiro.Casas.FirstOrDefault(c => c.Linha == casaAtual.Linha && c.Coluna == j);
                if (casa == null) break;
                if (casa.Peca == null)
                    movimentos.Add(new Movimento(this, casaAtual, casa));
                else
                {
                    if (casa.Peca.EBranca != this.EBranca)
                        movimentos.Add(new Movimento(this, casaAtual, casa, casa.Peca));
                    break;
                }
            }

            // Direção: Esquerda
            for (int j = casaAtual.Coluna - 1; j >= 0; j--)
            {
                var casa = tabuleiro.Casas.FirstOrDefault(c => c.Linha == casaAtual.Linha && c.Coluna == j);
                if (casa == null) break;
                if (casa.Peca == null)
                    movimentos.Add(new Movimento(this, casaAtual, casa));
                else
                {
                    if (casa.Peca.EBranca != this.EBranca)
                        movimentos.Add(new Movimento(this, casaAtual, casa, casa.Peca));
                    break;
                }
            }

            return movimentos;
        }

        /// <summary>
        /// Distribui as peças no tabuleiro no início do jogo.
        /// </summary>
        public void DistribuirPecas() 
        {
            // Duas torres para cada lado
            Casas.First(c => c.Linha == 0 && c.Coluna == 0).Peca = new Torre(true);
            Casas.First(c => c.Linha == 0 && c.Coluna == 7).Peca = new Torre(true);
            Casas.First(c => c.Linha == 7 && c.Coluna == 0).Peca = new Torre(false);
            Casas.First(c => c.Linha == 7 && c.Coluna == 7).Peca = new Torre(false);
        }

        /// <summary>
        /// Verifica se a torre pode se mover para a casa de destino.
        /// </summary>
        public bool ValidaMovimento(Casa destino, Tabuleiro tabuleiro)
        {
            return MovimentosPossiveis(tabuleiro).Any(m => m.Destino == destino);
        }

        /// <summary>
        /// Move a torre para a casa de destino, se o movimento for válido.
        /// </summary>
        public void ExecutaMovimento(Casa destino, Tabuleiro tabuleiro)
        {
            var casaAtual = tabuleiro.ObtemCasaPeca(this);
            if (casaAtual == null) throw new InvalidOperationException("A peça não está no tabuleiro.");

            if (!ValidaMovimento(destino, tabuleiro))
                throw new InvalidOperationException("Movimento inválido para a torre.");

            // Captura a peça (se houver)
            if (destino.Peca != null)
                tabuleiro.CapturarPeca(destino.Peca);

            // Move a torre
            destino.Peca = this;
            casaAtual.Peca = null;
        }

        /// <summary>
        /// Reverte um movimento no tabuleiro.
        /// </summary>
        public void ReverteMovimento(Movimento movimento) 
        {
            // Move a peça de volta para a origem
            movimento.Origem.Peca = movimento.Destino.Peca;
            movimento.Destino.Peca = movimento.PecaCapturada;

            // Remove a peça capturada da lista de capturas, se possivel
            if (movimento.PecaCapturada != null)
                PecasCapturadas.Remove(movimento.PecaCapturada); 
        }
    }
}
