using System.Collections.Generic; // Adicionado para List<>
using System.Linq; // Adicionado para FirstOrDefault
using Xadrez.Models; // Adicionado para Tabuleiro e Movimento, se não estiver globalmente

namespace Xadrez.Models.Pecas
{
    /// <summary>
    /// Representa a peça Cavalo no jogo de xadrez.
    /// </summary>
    public class Cavalo : Peca, ICavalo // Implementa a interface IPeca diretamente
    {
        /// <summary>
        /// Cria uma nova instância da peça Cavalo.
        /// </summary>
        /// <param name="eBranca">Indica se a peça é branca ou preta.</param>
        public Cavalo(bool eBranca) : base(eBranca){
            
        }
        /// <summary>
        /// Calcula e retorna todos os movimentos possíveis para o Cavalo
        /// a partir de sua posição atual no tabuleiro fornecido.
        /// </summary>
        /// <param name="tabuleiro">O estado atual do tabuleiro.</param>
        /// <returns>Uma lista de objetos Movimento representando os movimentos válidos.
        public override List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
        {
            var movimentosPossiveis = new List<Movimento>();

            Casa casaOrigem = tabuleiro.ObtemCasaPeca(this);
            if (casaOrigem == null)
            {
                // Se a peça não está no tabuleiro (situação inesperada), retorna lista vazia.
                return movimentosPossiveis;
            }

            // Pares representando os 8 movimentos possíveis do cavalo (deltaLinha, deltaColuna)
            int[,] deltas = new int[,]
            {
                { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 },
                { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 }
            };

            // O método GetLength(0) retorna o número de "linhas" na matriz 2D (que é 8)
            for (int i = 0; i < deltas.GetLength(0); i++)
            {
                int deltaLinha = deltas[i, 0];
                int deltaColuna = deltas[i, 1];

                int linhaDestino = casaOrigem.Linha + deltaLinha;
                int colunaDestino = casaOrigem.Coluna + deltaColuna;
                // Verifica se a nova posição está dentro dos limites do tabuleiro
                if (linhaDestino >= 0 && linhaDestino < 8 && colunaDestino >= 0 && colunaDestino < 8)
                {
                    // Se está dentro dos limites, encontrar a casa
                    Casa casaDestino = tabuleiro.Casas.FirstOrDefault(c => c.Linha == linhaDestino && c.Coluna == colunaDestino);

                    // Teoricamente, casaDestino nunca será null se os limites estão corretos,
                    // mas uma verificação extra pode ser segura se a criação do tabuleiro falhar.
                    if (casaDestino != null)
                    {
                        // 2. Verificar se a casa está vazia ou tem peça inimiga
                        if (casaDestino.Peca == null) // Se a casa estiver vazia (sem peça)
                        {
                            // Movimento válido para casa vazia
                            movimentosPossiveis.Add(new Movimento(this, casaOrigem, casaDestino));
                        }
                        else // Casa ocupada (tem peça)
                        {
                            // É uma peça inimiga? (Cor diferente da minha?)
                            if (casaDestino.Peca.EBranca != this.EBranca)
                            {
                                // Movimento válido com captura
                                movimentosPossiveis.Add(new Movimento(this, casaOrigem, casaDestino, casaDestino.Peca));
                            }
                            // Se for peça da mesma cor, não faz nada (movimento inválido)
                        }
                    }
                }
            }

            return movimentosPossiveis; // Retorna a lista com todos os movimentos válidos encontrados
        }
    }
}