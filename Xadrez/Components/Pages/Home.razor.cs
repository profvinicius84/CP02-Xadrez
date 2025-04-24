namespace Xadrez.Components.Pages
{
    public partial class Home
    {
        public Models.Jogador Jogador1 { get; set; }
        public Models.Jogador Jogador2 { get; set; }
        public Models.Partida<Models.Tabuleiro> Partida { get; set; }

        public Home()
        {
            Jogador1 = new Models.Jogador("Branco", true);
            Jogador2 = new Models.Jogador("Preto", false);
            Partida = new Models.Partida<Models.Tabuleiro>(Jogador1, Jogador2);
        }
    }
}