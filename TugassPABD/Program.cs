using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace TugassPABD
{
    class Program
    {
        static void Main(string[] args)
        {
            Program pr = new Program();
            string jawaban = Console.ReadLine();
            while (true)
            {
                try
                {
                    Console.WriteLine("Koneksi ke Database\n");
                    Console.WriteLine("Masukan User ID : ");
                    string user = Console.ReadLine();
                    Console.WriteLine("Masukan password : ");
                    string pass = Console.ReadLine();
                    Console.WriteLine("Masukan database tujuan : ");
                    string db = Console.ReadLine();
                    Console.WriteLine("\nketik k ntuk terhubung ke database : ");
                    char chr = Convert.ToChar(Console.ReadLine());
                    switch (chr)
                    {
                        case 'k':
                            {
                                SqlConnection conn = null;
                                string strkoneksi = "data source = TASYA\\TASYA_MASTA; " +
                                    "initial catalog = {0}; " +
                                    "user ID = {1}; password = {2}";
                                conn = new SqlConnection(string.Format(strkoneksi, db, user, pass));
                                conn.Open();
                                Console.Clear();
                                while (true)
                                {
                                    try
                                    {

                                        Console.WriteLine("\nMenu");
                                        Console.WriteLine("1. Melihat Seluruh Data");
                                        Console.WriteLine("2. Tambah Data");
                                        Console.WriteLine("3. Hapus Data");
                                        Console.WriteLine("4. Cari Data");
                                        Console.WriteLine("5. Keluar");
                                        Console.Write("\nEnter your choice (1-5): ");
                                        char ch = Convert.ToChar(Console.ReadLine());
                                        switch (ch)
                                        {
                                            case '1':
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("DATA donatur\n");
                                                    Console.WriteLine();
                                                    pr.baca(conn);
                                                }
                                                break;
                                            case '2':
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("INPUT DATA donatur\n");
                                                    Console.Write("Masukan id donatur : ");
                                                    string id = Console.ReadLine();
                                                    Console.Write("Masukan Nama depan donatur : ");
                                                    string Nmadep = Console.ReadLine();
                                                    Console.Write("Masukan Nama belakang donatur : ");
                                                    string Nmabel = Console.ReadLine();
                                                    Console.Write("Masukan no telp donatur : ");
                                                    string notelp = Console.ReadLine();
                                                    if (id.Equals(pr.search(id, conn)))
                                                    {
                                                        Console.WriteLine("Data Sudah Ada");
                                                    }
                                                    else
                                                    {
                                                        try
                                                        {
                                                            pr.insert(id, Nmadep, Nmabel, notelp , conn);
                                                        }
                                                        catch
                                                        {
                                                            Console.WriteLine("\nData tidak boleh double");
                                                        }
                                                    }
                                                }
                                                break;
                                            case '3':
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("Delete DATA donatur\n");
                                                    Console.Write("Masukan id : ");
                                                    string id = Console.ReadLine();
                                                    Console.WriteLine("Apakah anda yakin ingin menghapus data ini?");
                                                    Console.WriteLine("y/n : ");
                                                    jawaban = Console.ReadLine();
                                                    if (jawaban.Equals("y"))
                                                    {
                                                        try
                                                        {
                                                            pr.delete(id, conn);
                                                        }
                                                        catch
                                                        {
                                                            Console.WriteLine("\nData telah dihapus");
                                                        }
                                                    }
                                                    else break;
                                                }
                                                break;
                                            case '4':
                                                {
                                                    Console.Write("Masukan id donatur : ");
                                                    string id = Console.ReadLine();

                                                }
                                                break;

                                            case '5':
                                                {
                                                    conn.Close();
                                                    return;
                                                }
                                                break;
                                            default:
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("\nInvalid option");
                                                }
                                                break;
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nCheck for the value entered.");
                                    }
                                }
                            }
                        default:
                            {
                                Console.WriteLine("\nInvalid Option");
                            }
                            break;
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Tidak dapat mengakses database menggunakan user tersebut\n");
                    Console.ResetColor();
                }
            }
        }
        public void baca(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("Select * From donatur", con);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                for (int i = 0; i < r.FieldCount; i++)
                {
                    Console.WriteLine(r.GetValue(i));
                }
                Console.WriteLine();
            }
            r.Close();
        }
        public void insert(string id, string Nmadep, string Nmabel,string notelp, SqlConnection con)
        {
            string str = "";
            str = "insert into donatur (id_donatur,nama_depan_donatur, nama_belakang_donatur, notelp_donatur)values(@id,@nmadep , @nmabel, @notelp)";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("id_donatur", id));
            cmd.Parameters.Add(new SqlParameter("nama_depan_donatur", Nmadep));
            cmd.Parameters.Add(new SqlParameter("nama_belakang_donatur", Nmabel));
            cmd.Parameters.Add(new SqlParameter("notelp_donatur", notelp));

            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Berhasil ditambahkan");
        }
        public void delete(string id, SqlConnection con)
        {
            string str = "";
            str = "delete from donatur where id = @id";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("id", id));
            cmd.ExecuteNonQuery();
            Console.WriteLine("\nData telah dihapus");

        }
        public string search(string id, SqlConnection con)
        {
            string str = "";
            string ID = "";
            str = "select nim from donatur where id = @id";
            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("id", id));
            cmd.ExecuteNonQuery();
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                id = r.GetValue(0).ToString();
                Console.WriteLine();

            }
            r.Close();
            return str;

        }
    }
}

