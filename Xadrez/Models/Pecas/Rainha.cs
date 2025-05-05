using System.Collections.Generic;
using System.Linq;
using Xadrez.Models;

namespace Xadrez.Models.Pecas
{
    /// <summary>
    /// Representa a peça Rainha no jogo de xadrez.
    /// A Rainha combina os movimentos da torre e do bispo, podendo se mover
    /// qualquer número de casas em linha reta — vertical, horizontal ou diagonal —
    /// desde que o caminho esteja desobstruído.
    /// </summary>
    public class Rainha : Peca, IRainha
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Rainha"/>.
        /// </summary>
        /// <param name="eBranca">Indica se a peça é branca (<c>true</c>) ou preta (<c>false</c>).</param>
        public Rainha(bool eBranca) : base(eBranca)
        {
        }

        /// <summary>
        /// Calcula e retorna todos os movimentos válidos que a Rainha pode realizar
        /// a partir de sua posição atual no tabuleiro fornecido.
        /// </summary>
        /// <param name="tabuleiro">O estado atual do tabuleiro.</param>
        /// <returns>
        /// Uma lista de objetos <see cref="Movimento"/> representando todos os movimentos
        /// válidos disponíveis para a Rainha nesta jogada.
        /// </returns>
        public override List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
        {
            var movimentosPossiveis = new List<Movimento>();

            Casa casaOrigem = tabuleiro.ObtemCasaPeca(this);
            if (casaOrigem == null)
            {
                return movimentosPossiveis; // A Rainha não está posicionada no tabuleiro
            }

            // Direções possíveis: vertical, horizontal e diagonal
            int[,] direcoes = new int[,]
            {
                { -1, 0 }, { 1, 0 },   // vertical: cima e baixo
                { 0, -1 }, { 0, 1 },   // horizontal: esquerda e direita
                { -1, -1 }, { -1, 1 }, // diagonal: cima-esquerda e cima-direita
                { 1, -1 }, { 1, 1 }    // diagonal: baixo-esquerda e baixo-direita
            };

            for (int i = 0; i < direcoes.GetLength(0); i++)
            {
                int deltaLinha = direcoes[i, 0];
                int deltaColuna = direcoes[i, 1];

                int linhaDestino = casaOrigem.Linha + deltaLinha;
                int colunaDestino = casaOrigem.Coluna + deltaColuna;

                // Continua na direção enquanto estiver dentro dos limites do tabuleiro
                while (linhaDestino >= 0 && linhaDestino < 8 &&
                       colunaDestino >= 0 && colunaDestino < 8)
                {
                    Casa casaDestino = tabuleiro.Casas.FirstOrDefault(
                        c => c.Linha == linhaDestino && c.Coluna == colunaDestino);

                    if (casaDestino != null)
                    {
                        if (casaDestino.Peca == null)
                        {
                            // Casa livre: adicionar movimento
                            movimentosPossiveis.Add(new Movimento(this, casaOrigem, casaDestino));
                        }
                        else
                        {
                            // Casa ocupada: se for inimiga, pode capturar
                            if (casaDestino.Peca.EBranca != this.EBranca)
                            {
                                movimentosPossiveis.Add(new Movimento(this, casaOrigem, casaDestino, casaDestino.Peca));
                            }
                            break; // Interrompe a varredura após encontrar qualquer peça
                        }
                    }

                    linhaDestino += deltaLinha;
                    colunaDestino += deltaColuna;
                }
            }

            return movimentosPossiveis;
        }
    }
}
