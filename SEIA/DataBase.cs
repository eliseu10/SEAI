using System;
using Npgsql;


namespace SEIA
{
	public class DataBase
	{
		//int  nMonths, nShots, maxNshots ;
		Int64 age, nFlaws, maxNflaws;
		NpgsqlConnection conn = new NpgsqlConnection(
			"Server=packy.db.elephantsql.com;" +
			"Port=5432;" +
			"UserId=nyetjoxs;" +
			"Password=yMHcm5QqQHvsKPL6bQKmWNyTlLJWRNwO;" +
			"Database=nyetjoxs");
		//"Server=db.fe.up.pt;Port=5432;UserId=up201306445;Password=obaptisteegay;Database=up201306445"


		public Int64 SearchAge(int ID){
			
			conn.Open();

			string query = "SELECT age FROM equipments WHERE equipmentID = " + ID;
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			NpgsqlDataReader dr = cmd.ExecuteReader();

			while (dr.Read())
				age = dr.GetInt32(0);

			conn.Close();
			return age;

		}



		//n de falhas
		public Int64 SearchNumberFlaws(int ID){
			conn.Open();

			string query = "SELECT count(*) FROM flaws WHERE equipmentID = " + ID;
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			nFlaws = (Int64)cmd.ExecuteScalar();

			conn.Close();
			return nFlaws;
		}



		/*
		//n de disparos do dijuntor
		public int SearchNumberShots(int ID){
			this.Connect ();

			string query = "SELECT nshots FROM components WHERE componentID = " + ID;
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			var reader = cmd.ExecuteReader();
			nShots = int.Parse (reader.ToString ());

			this.CloseConnection ();
			return nShots;
		}

		//n de meses desde a ultima operacao(disparo)
		public int SearchMonthsLastOp(){
			this.Connect ();

			string query = "SELECT";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			var reader = cmd.ExecuteReader();
			nMonths = int.Parse (reader.ToString ());

			this.CloseConnection ();
			return nMonths;
		}

		//n maximo de disparos em todos os dijuntores
		public int SearchMaxNumberShots(){
			this.Connect ();

			string query = "SELECT";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			var reader = cmd.ExecuteReader();
			maxNshots = int.Parse (reader.ToString ());

			this.CloseConnection ();
			return maxNshots;
		}*/

		//n maximo de falhas em todos os dijuntores
		public Int64 SearchMaxNumberFlaws(){
			conn.Open();

			string query = "SELECT count(flawID), equipmentid FROM public.flaws GROUP BY equipmentid ORDER BY count(flawID) DESC";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			NpgsqlDataReader dr = cmd.ExecuteReader();
			//maxNflaws = (Int64)cmd.ExecuteScalar();

			while (dr.Read ()) {
				maxNflaws = dr.GetInt64(0);
				break;
			}
				

			conn.Close ();
			return maxNflaws;
		}

	}
}

