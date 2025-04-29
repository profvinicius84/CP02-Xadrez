using System.Collections.Generic;
using System.Linq;
using Xadrez.Models;

namespace Xadrez.Models.Pecas
{
    /// <summary>
    /// Representa a peça Rainha no jogo de xadrez.
    /// A rainha pode se mover em qualquer direção (vertical, horizontal e diagonal) quantas casas quiser.
    /// </summary>
    public class Rainha : Peca, IRainha
    {
        /// <summary>
        /// Cria uma nova instância da peça Rainha.
        /// </summary>
        /// <param name="eBranca">Indica se a rainha é branca ou preta.</param>
        public Rainha(bool eBranca) : base(eBranca)
        {
        }

        /// <summary>
        /// Calcula e retorna todos os movimentos possíveis para a Rainha
        /// a partir de sua posição atual no tabuleiro fornecido.
        /// </summary>
        /// <param name="tabuleiro">O estado atual do tabuleiro.</param>
        /// <returns>Uma lista de objetos Movimento representando os movimentos válidos.</returns>
        public override List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
        {
            var movimentosPossiveis = new List<Movimento>();

            Casa casaOrigem = tabuleiro.ObtemCasaPeca(this);
            if (casaOrigem == null)
            {
                return movimentosPossiveis; // Rainha não está no tabuleiro
            }

            // 8 direções possíveis: verticais, horizontais e diagonais
            int[,] direcoes = new int[,]
            {
                { -1, 0 }, { 1, 0 },   // cima, baixo
                { 0, -1 }, { 0, 1 },   // esquerda, direita
                { -1, -1 }, { -1, 1 }, // diagonais cima-esquerda e cima-direita
                { 1, -1 }, { 1, 1 }    // diagonais baixo-esquerda e baixo-direita
            };

            for (int i = 0; i < direcoes.GetLength(0); i++)
            {
                int deltaLinha = direcoes[i, 0];
                int deltaColuna = direcoes[i, 1];

                int linhaDestino = casaOrigem.Linha + deltaLinha;
                int colunaDestino = casaOrigem.Coluna + deltaColuna;

                // Enquanto estiver dentro dos limites do tabuleiro
                while (linhaDestino >= 0 && linhaDestino < 8 && colunaDestino >= 0 && colunaDestino < 8)
                {
                    Casa casaDestino = tabuleiro.Casas.FirstOrDefault(c => c.Linha == linhaDestino && c.Coluna == colunaDestino);

                    if (casaDestino != null)
                    {
                        if (casaDestino.Peca == null)
                        {
                            movimentosPossiveis.Add(new Movimento(this, casaOrigem, casaDestino));
                        }
                        else
                        {
                            if (casaDestino.Peca.EBranca != this.EBranca)
                            {
                                movimentosPossiveis.Add(new Movimento(this, casaOrigem, casaDestino, casaDestino.Peca));
                            }
                            break; // Parar ao encontrar qualquer peça
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
