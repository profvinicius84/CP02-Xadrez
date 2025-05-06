
namespace Xadrez.Models.Pecas;

/// <summary>
/// Interface que representa uma peça de xadrez do tipo Torre.
/// </summary>
public class Torre(bool eBranca) : Peca(eBranca), ITorre
{
    public override List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
    {
        //throw new NotImplementedException();
        return new List<Movimento>();
    }
}