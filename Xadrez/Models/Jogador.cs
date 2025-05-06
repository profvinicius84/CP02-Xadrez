namespace Xadrez.Models;

/// <summary>
/// Representa um jogador de xadrez.
/// </summary>
/// <param name="nome">Representa o nome do jogador.</param>
/// <param name="eBranco">Representa se o jogador é branco ou não.</param>
public class Jogador(string nome, bool eBranco)
{
    /// <summary>
    /// Representa o nome do jogador.
    /// </summary>
    public string Nome { get; set; } = nome;

    /// <summary>
    /// Representa se o jogador é branco ou não.
    /// </summary>
    public bool EBranco { get; set; } = eBranco;
}