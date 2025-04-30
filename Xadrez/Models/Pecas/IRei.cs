namespace Xadrez.Models.Pecas;

/// <summary>
/// Interface que representa um rei de xadrez.
/// </summary>
public interface IRei :IPeca
{
    /// <summary>
    /// Verifica se o rei pode fazer o roque (pequeno ou grande).
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
    /// <param name="roquePequeno">Indica se o roque é pequeno ou grande. O roque pequeno é feito com a torre do lado do rei e o roque grande é feito com a torre do lado da dama.</param>
    /// <returns>Retorna verdadeiro se o roque é possível, caso contrário, retorna falso.</returns>
    bool VerificaRoque(ITabuleiro tabuleiro, bool roquePequeno = false);

    /// <summary>
    /// Executa o roque (pequeno ou grande) no tabuleiro.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
    /// <param name="roquePequeno">Indica se o roque é pequeno ou grande. O roque pequeno é feito com a torre do lado do rei e o roque grande é feito com a torre do lado da dama.</param>
    /// <returns>Retorna o movimento executado pelo roque. O movimento é composto pelo rei e pela torre que se movem ao mesmo tempo.</returns>
    Movimento ExecutaRoque(ITabuleiro tabuleiro, bool roquePequeno = false);
}
