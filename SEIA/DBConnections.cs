using System;
using Npgsql;


namespace SEIA
{
	public class DBConnect
	{
		NpgsqlConnection conn = new NpgsqlConnection("Server=db.fe.up.pt;Port=5432;UserId=up201306445;Password=obaptisteegay;Database=up201306445;");
		Int64 age, maxNflaws;

		/********************************** Basic Functions **********************************/
		// Abre coneção
		public void OpenConn()
		{
			try
			{
				conn.Open();

				//não funciona, recorer a \"SEAI\".\"EXAMPLE_TABLE\"
				/*
                string query = "SET SCHEMA 'SEAI'"; // define o schema do trabalho
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                */

				//Console.WriteLine("Coneção bem sucedida!");
			}
			catch (Exception)
			{
				Console.WriteLine("Erro a abrir coneção!");
			}
		}

		// Termina Coneção
		public void CloseConn()
		{
			try
			{
				conn.Close();
				// Console.WriteLine("Coneção concluída!\n");
			}
			catch (Exception)
			{
				Console.WriteLine("Erro a fechar coneção!");
			}
		}

		// Verifica se um parametro com um determinado id existe 
		public bool VerifyId(int id, string table)
		{
			//this.OpenConn();

			string query = $"SELECT COUNT(*) FROM \"SEAI\".\"{table}\" WHERE \"ID\" = {id}";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			Int64 result = (Int64)cmd.ExecuteScalar();
			string result_s = result.ToString();

			if (result == 0)
			{
				//this.CloseConn();
				Console.WriteLine($"Erro: Não existe o ID nº {id} na tabela \"{table}\"!\n");
				return false;
			}
			else
			{
				//this.CloseConn();
				//Console.WriteLine($"Existe o ID nº {id} na tabela \"{table}\"!");
				return true;
			}

		}


		/********************************** Query Functions **********************************/
		// Nº de falhas de um equipamento, retorna -1 se não existir o equipamento
		public Int64 NumberFlaws(int id)
		{
			this.OpenConn();
			if (VerifyId(id, "Equipments") == false)
			{
				this.CloseConn();
				return -1;
			}

			string query = $"SELECT COUNT(*) FROM \"SEAI\".\"Flaws\" WHERE \"EquipmentID\" = {id}";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			Int64 nFlaws = (Int64)cmd.ExecuteScalar();

			if (nFlaws == 0)
				Console.WriteLine($"O equipamento {id} nunca teve uma falha!");
			else
				Console.WriteLine($"O equipamento {id} teve {nFlaws} falha(s) até à data!");

			this.CloseConn();
			return nFlaws;
		}

		// Idade de um equipamento em meses, retorna -1 se não existir o equipamento
		public Int64 Age(int id)
		{
			this.OpenConn();
			if (VerifyId(id, "Equipments") == false)
			{
				this.CloseConn();
				return -1;
			}

			string query = $"SELECT \"Year\" FROM \"SEAI\".\"Equipments\" WHERE \"Equipments\".\"ID\" = {id}";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			NpgsqlDataReader dr = cmd.ExecuteReader();

			while (dr.Read())
				age = dr.GetInt32(0);

			Console.WriteLine($"O equipamento {id} tem {age} ano(s) em regime de trabalho!");

			this.CloseConn();
			return age;
		}


		// Nº de falhas em todos os dijuntores (!)
		public Int64 TotalFlaws()
		{
			this.OpenConn();
			string query = "SELECT count(\"SEAI\".\"Flaws\".\"ID\"), \"SEAI\".\"Equipments\".\"ID\" FROM public.flaws GROUP BY equipmentid ORDER BY count(flawID) DESC";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			NpgsqlDataReader dr = cmd.ExecuteReader();

			while (dr.Read())
			{
				maxNflaws = dr.GetInt64(0);
				break;
			}

			this.CloseConn();
			return maxNflaws;
		}
		// (!)
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


		/********************************** Edition Functions **********************************/
		//Adicionar/Remover Falha (!)
		public void insertFlaw(int flawID, int equipmentID, int cause, int seriousness){
			/*
			 * INSERT INTO "SEAI"."Flaws" ("EquipmentID", "Cause", "Seriousness")
VALUES (2,2,4); */
		}

		public void removeFlaw(int flawID){

		}
			
		//Adicionar/Remover Equipamento (!)
		public void insertEquipment(int type, String name, int year){
			this.OpenConn();
			string query = $"INSERT INTO \"SEAI\".\"Equipments\" (\"Type\", \"Name\", \"Year\") VALUES ({type},'{name}',{year});";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			cmd.ExecuteNonQuery();
			Console.WriteLine($"Foi inserido equipamento {name}, do tipo {type}, do ano de {year}.");
			this.CloseConn();
		}

		public void removeEquipment(int equipmentID){
			this.OpenConn();
			string query = $"DELETE FROM \"SEAI\".\"Equipments\" WHERE \"Equipments\".\"ID\" = {equipmentID};";
			NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
			cmd.ExecuteNonQuery();
			Console.WriteLine($"Foi removido o equipamento com o ID {equipmentID}.");
			this.CloseConn();
		}

		//Atualizar Equipamento (!)

		//Adicionar/Remover Sample (!)
		public void insertSample(){

		}

		public void removeSample(int sampleID){

		}

		/********************************** E N D **********************************/

	}
}

