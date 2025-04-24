using Xadrez.Models.Pecas;

namespace Xadrez.Models;

/// <summary>
/// Representa uma casa no tabuleiro de xadrez.
/// </summary>
/// <param name="linha">Representa a linha da casa no tabuleiro.</param>
/// <param name="coluna">Representa a coluna da casa no tabuleiro.</param>
public class Casa(int linha, int coluna)
{
    /// <summary>
    /// Representa a linha da casa no tabuleiro.
    /// </summary>
    public int Linha { get; } = linha;

    /// <summary>
    /// Representa a coluna da casa no tabuleiro.
    /// </summary>
    public int Coluna { get; } = coluna;

    /// <summary>
    /// Representa o código da casa no tabuleiro (ex: A1, B2, etc.).
    /// </summary>
    public string Codigo => $"{(char)(Coluna + 65)}{Linha + 1}";

    /// <summary>
    /// Representa a cor da casa no tabuleiro.
    /// </summary>
    public string Cor => (Linha + Coluna) % 2 == 0 ? "Branca" : "Preta";    

    /// <summary>
    /// Representa a peça que ocupa a casa, se houver.
    /// </summary>
    public IPeca? Peca { get; set; }
}