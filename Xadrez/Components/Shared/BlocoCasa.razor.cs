using Microsoft.AspNetCore.Components;
using Xadrez.Models.Pecas;

namespace Xadrez.Components.Shared;

/// <summary>
/// Classe que representa um bloco de casa no tabuleiro de xadrez.
/// </summary>
public partial class BlocoCasa
{
    /// <summary>
    /// Representa a casa do tabuleiro de xadrez.
    /// </summary>
    [Parameter]
    public Models.Casa Casa { get; set; }

    /// <summary>
    /// Indica se a casa está disponível para seleção.
    /// </summary>
    [Parameter]
    public string CondicaoDisponibilidade { get; set; }

    /// <summary>
    /// Evento que é chamado quando a casa é selecionada.
    /// </summary>
    [Parameter]
    public EventCallback AoSelecionar { get; set; }

    /// <summary>
    /// Icone da peça que ocupa a casa, se houver.
    /// </summary>
    public string IconePeca
    {
        get
        {
            if (Casa.Peca is null)
                return "";
            string icone = "fa-solid ";
            switch (Casa.Peca)
            {
                case IPeao:
                    icone += "fa-chess-pawn";
                    break;
                case ITorre:
                    icone += "fa-chess-rook";
                    break;
                case ICavalo:
                    icone += "fa-chess-knight";
                    break;
                case IBispo:
                    icone += "fa-chess-bishop";
                    break;
                case IRainha:
                    icone += "fa-chess-queen";
                    break;
                case IRei:
                    icone += "fa-chess-king";
                    break;
                default:
                    icone += "fa-skull";
                    break;
            }            
            return icone;
        }
    }

    /// <summary>
    /// Retorna se a casa está ocupada ou não.
    /// </summary>
    public string StatusOcupcao
    {
        get
        {
            return Casa.Peca is null ? "vazia" : "ocupada";
        }
    }    
}