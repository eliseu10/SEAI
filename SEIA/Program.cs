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

			dbconnect.insertEquipment (0,"D",1990,20,30);
			dbconnect.insertEquipment (3,"Disjuntor",1991,30,23);
			dbconnect.insertEquipment (2,"Disj",2010,34,54);
			dbconnect.insertEquipment (3,"DisjDoCanto",2001,56,67);

			dbconnect.insertLastUseDate (3,"10.02.2010 12:02:59");

			dbconnect.insertFlaw (3,0,3);
			dbconnect.removeFlaw (2);

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
				
			dbconnect.removeEquipment (1);
			dbconnect.removeEquipment (2);

			// Keep the console window open in debug mode.
			Console.WriteLine("\nPress any key to exit.");
			Console.ReadKey();
		}
	}
}
