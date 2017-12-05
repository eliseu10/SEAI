using System;
using Npgsql;

namespace SEIA
{
	class MainClass
	{
		
		public static void Main (string[] args)
		{
			/*
			var connString = ;
			var conn = new NpgsqlConnection (connString);
			conn.Open ();
			var cmd = new NpgsqlCommand ("SELECT count(*) FROM equipments", conn);
			Int64 count = (Int64)cmd.ExecuteScalar();

			Console.Write("{0}\n", count);
			conn.Close();*/


			DataBase db = new DataBase ();
			Int64 age = db.SearchAge (1);
			Console.WriteLine ("Idade: " + age);

			Int64 nFlaws = db.SearchNumberFlaws(1);
			Console.WriteLine ("N Relatorios: " + nFlaws);

			Int64 nMaxFlaws = db.SearchMaxNumberFlaws();
			Console.WriteLine ("N Maximo Relatorios: " + nMaxFlaws);

			//Console.WriteLine (age.ToString() );

		}
	}
}
