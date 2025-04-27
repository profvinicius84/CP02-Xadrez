using System.Collections.Generic; // Adicionado para List<>
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
            // TODO: Implementar a lógica real do movimento do cavalo aqui.
            var movimentosPossiveis = new List<Movimento>();

            // Lógica para calcular os 8 movimentos possíveis do cavalo será adicionada aqui...
            return movimentosPossiveis; // Retorna a lista (vazia por enquanto)
        }
    }
}