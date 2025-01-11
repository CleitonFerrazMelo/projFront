using System.Text;

namespace projFront.Helpers
{
	public static class ManipularString
	{
		public static string QuebrarTexto(string texto, int maxCaracteresPorLinha)
		{
			string[] palavras = texto.Split(' ');
			StringBuilder novoTexto = new StringBuilder();
			int caracteresNaLinha = 0;

			foreach (string palavra in palavras)
			{
				if (caracteresNaLinha + palavra.Length > maxCaracteresPorLinha)
				{
					novoTexto.AppendLine("<br>");
					caracteresNaLinha = 0;
				}

				novoTexto.Append(palavra + " ");
				caracteresNaLinha += palavra.Length + 1; // +1 para o espaço em branco
			}

			return novoTexto.ToString();
		}
	}
}
