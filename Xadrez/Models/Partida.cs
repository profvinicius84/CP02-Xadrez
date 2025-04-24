namespace Xadrez.Models;

/// <summary>
/// Representa uma partida de xadrez.
/// </summary>
public class Partida<TTabuleiro> where TTabuleiro : ITabuleiro, new()
{
    /// <summary>
    /// Representa o ID da partida.
    /// </summary>
    public Guid Id { get; } = new();

    /// <summary>
    /// Representa o jogador branco.
    /// </summary>
    public Jogador JogadorBranco { get; set; }

    /// <summary>
    /// Representa o jogador preto.
    /// </summary>
    public Jogador JogadorPreto { get; set; }

    /// <summary>
    /// Representa o tabuleiro da partida.
    /// </summary>
    public TTabuleiro Tabuleiro { get; set; } = new();
   

    /// <summary>
    /// Movimentos realizados na partida.
    /// </summary>
    public List<Movimento> Movimentos { get; set; } = new();

    public Jogador JogadorDaVez => Movimentos.Count % 2 == 0 ? JogadorBranco : JogadorPreto;

    /// <summary>
    /// Construtor da classe Partida.
    /// </summary>
    /// <param name="jogadorBranco"></param>
    /// <param name="jogadorPreto"></param>
    public Partida(Jogador jogadorBranco, Jogador jogadorPreto)
    {
        JogadorBranco = jogadorBranco;
        JogadorPreto = jogadorPreto;
        Tabuleiro.DistribuiPecas();
    }
}