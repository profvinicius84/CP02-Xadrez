namespace Xadrez.Models.Pecas;

/// <summary>
/// Interface que representa um rei de xadrez.
/// </summary>
public interface IRei
{
    /// <summary>
    /// Indica se o rei está em cheque ou não.
    /// </summary>
    bool EmCheque { get; set; }

    /// <summary>
    /// Verifica se o rei pode fazer o roque (pequeno ou grande).
    /// </summary>
    /// <param name="tabuleiro"></param>
    /// <param name="roquePequeno"></param>
    /// <returns></returns>
    bool VerificaRoque(Tabuleiro tabuleiro, bool roquePequeno = false);

    /// <summary>
    /// Executa o roque (pequeno ou grande) no tabuleiro.
    /// </summary>
    /// <param name="tabuleiro"></param>
    /// <param name="roquePequeno"></param>
    /// <returns></returns>
    Movimento ExecutaRoque(Tabuleiro tabuleiro, bool roquePequeno = false);
}
