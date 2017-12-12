using System;
using Npgsql;

namespace SEIA
{
	class MainClass
	{
		
		public static void Main (string[] args)
		{
			//--------------------------------
			// Teste de acesso à Base de Dados
			//--------------------------------
			Console.WriteLine("----------------------\nTeste da Base de Dados \n----------------------\n");
			DBConnect dbconnect = new DBConnect();

			// Teste nº falhas equipamentos 1 ao 4
			if(true)
			{
				dbconnect.NumberFlaws(1); // 1 erro
				dbconnect.NumberFlaws(2); // 2 erros
				dbconnect.NumberFlaws(3); // sem erros
				dbconnect.NumberFlaws(4); // n existe
			}

			// Teste idade equipamentos 1 ao 4
			if(true)
			{
				dbconnect.Age(1); // 3 anos
				dbconnect.Age(2); // 1 ano
				dbconnect.Age(3); // 1 ano
				dbconnect.Age(4); // n existe
			}

			if(true)
			{
				dbconnect.insertEquipment (3,"DisjuntorDoCanto",1999);
			}

			// Keep the console window open in debug mode.
			Console.WriteLine("\nPress any key to exit.");
			Console.ReadKey();
		}
	}
}
