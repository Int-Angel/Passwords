using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Passwords
{
    class Program
    {
        static void Main(string[] args)
        {
            int size, mayus, specChars, cantidad;
            bool rep; //sirve para verificar los ciclos, indica si se tiene que repetir alguno
            do
            {
                Console.Clear();
                do
                {
                    Console.Clear();
                    Console.Write("Cuantos passwords desea generar?");
                    cantidad = int.Parse(Console.ReadLine());
                    if (cantidad < 1)
                    {
                        rep = true;
                        Console.Write("La cantidad no puede ser 0 o menor...");
                        Console.ReadKey();
                    }
                    else
                    {
                        rep = false;
                    }
                } while (rep);
                
                do
                {
                    Console.Clear();
                    Console.Write("De que tamaño será cada password?");
                    size = int.Parse(Console.ReadLine());
                    if (size < 1)
                    {
                        rep = true;
                        Console.Write("El tamaño no puede ser 0 o menor...");
                        Console.ReadKey();
                    }else
                    {
                        rep = false;
                    }
                } while (rep);
                
                Console.Write("Cuántas mayúsculas debo poner en los passwords?");
                mayus = int.Parse(Console.ReadLine());
                Console.Write("Cuántos caracteres especiales debo poner en los passwords?");
                specChars = int.Parse(Console.ReadLine());
                if ((specChars + mayus) > size)
                {
                    rep = true;
                    Console.WriteLine("La suma de las mayusculas y de los caracteres especiales no puede ser mayor al de el tamaño...");
                    Console.ReadKey();
                }else
                {
                    rep = false;
                }
            } while (rep);
            
            
            Password p = new Password(cantidad,size,mayus,specChars);
            
        }
    }

    class Password
    {
        private int mPassSize, mNumberOfMayus, mNumberOfSpecChars, n, interval; //n es la cantidad de passwords a generar
        private string[] normalPasswords;
        private string[] encryptedPasswords;
        private string[] desencryptedPasswords;

        public Password(int cantidad, int _mPassSize, int _mNumberOfMayus, int _mNumberofSpecChars)
        {
            n = cantidad;
            mPassSize = _mPassSize;
            mNumberOfMayus = _mNumberOfMayus;
            mNumberOfSpecChars = _mNumberofSpecChars;
            Random random = new Random();
            //se genera random el intervalo para encriptar
            interval = random.Next(255); // creo que es suficiente hasta 255
            start();
        }

        public void start()
        {
            generatePassword(n);
            foreach (var password in normalPasswords)
            {
                Console.WriteLine(password);
            }
            Console.WriteLine("\n");
            Console.ReadKey();
            
            encryptPasswds(interval);
            foreach (var password in encryptedPasswords)
            {
                Console.WriteLine(password);
            }
            Console.WriteLine("\n");
            Console.ReadKey();
            
            decryptPasswds(interval);
            foreach (var password in desencryptedPasswords)
            {
                Console.WriteLine(password);
            }
            Console.WriteLine("\n");
            Console.ReadKey();
        }

        private void generatePassword(int numberofPasswords)
        {
            
          normalPasswords = new string[numberofPasswords];
          creador();
          
        }

        private void creador(int pos = 0)
        {
            
            string tempPass=""; //password temporal
            Random random = new Random();
            for (int i = 0; i < mNumberOfMayus; i++)
            {
                tempPass += Convert.ToChar(random.Next(65, 90));  //agregamos todas las mayusculas
            }
           
            for (int i = 0; i < mNumberOfSpecChars; i++)
            {
                tempPass += Convert.ToChar(random.Next(33, 64));  //agregamos caracteres especiales
            }
            
            for (int i = 0; i < mPassSize-mNumberOfMayus-mNumberOfSpecChars; i++)
            {
                tempPass += Convert.ToChar(random.Next(97, 122));  //completamos con caracteres que falten con letras minusculas
            }
            
            //revolvemos intercambiando caracteres random
            char aux;
            char[] tempArray = new char[mPassSize];
            int alt; // guarda el numero random generado para volverlo a usar
            tempArray = tempPass.ToCharArray();
            for (int i = 0; i < mPassSize; i++)
            {
                aux = tempArray[i];
                alt = random.Next(0, mPassSize - 1);
                tempArray[i] = tempArray[alt];
                tempArray[alt] = aux;

            }
            
            tempPass = "";
            foreach (var VARIABLE in tempArray)
            {
                tempPass += VARIABLE;
            }
            /*
             * Antes qui imprimia la contraseña y ponina un Console.ReadKey(), pero al momento de quitarlo las contraseñas empezaron a ser las mismas simpre, nose porque
             * entonces puse un pequeño delay entre cada password
             */
            Thread.Sleep(100);  
            normalPasswords[pos] = tempPass;
            if (pos == n-1)
            {
                return;
            }
           
            creador(++pos);
        }

        public void encryptPasswds(int interval)
        {
            encryptedPasswords = new string[n];
            char[] tempArray; // guarda la contarseña normal en forma de arreglo
            char[] encryptedArray; // guarda los caracteres de la contraseñ anormal encriptados
            string tempPass = ""; // junta el arrglo anterior en esta string para  despues guardar esta string en el arreglo de contraseñas encriptadas
            int letraInt; // letra de tempArray en forma de entero (dependiendo del ciclo toma su valor)
            int j = 0; // iterador
            
            foreach (var pass in normalPasswords)
            {
                tempArray = pass.ToCharArray();
                encryptedArray = new char[pass.Length];
             
                for (int i = 0; i < tempArray.Length; i++)
                {
                    letraInt = Convert.ToInt32(tempArray[i]);
                    letraInt += interval;
                    if (letraInt > 255) // si se pasa de 255 le restamos 256 que es el numero de caracteres para que de la vuelta a la tabla
                    {
                        letraInt -= 256;
                    }

                    encryptedArray[i] = Convert.ToChar(letraInt);
                }

                tempPass = "";
                foreach (var VARIABLE in encryptedArray)
                {
                    tempPass += VARIABLE;
                }

                encryptedPasswords[j] = tempPass;
                j++;
            }
        }

        public void decryptPasswds(int interval)
        {
            desencryptedPasswords = new string[n];
            char[] tempArray; //guarda la contraseña encriptada en forma de un arreglo de caracteres para poder trabajarlo
            char[] desencryptedArray; // guarda en los caracteres ya desencriptados
            string tempPass = ""; // junta desencryptedArray para luego guardarlo en el arreglo de contraseñas desencriptadas
            int letraInt;  //es la letra en la que estamos del arreglo en entero
            int j = 0;  //iterador
            
            foreach (var pass in encryptedPasswords)
            {
                tempArray = pass.ToCharArray();
                desencryptedArray = new char[pass.Length];
             
                for (int i = 0; i < tempArray.Length; i++)
                {
                    letraInt = Convert.ToInt32(tempArray[i]);
                    letraInt -= interval; //deencriptamos 
                    if (letraInt < 0) // si obtenemos numeros negativos le sumamos 256 que es el numero de caracteres para que le de vuelta a la tabla
                    {
                        letraInt += 256;
                    }

                    desencryptedArray[i] = Convert.ToChar(letraInt);
                }

                tempPass = "";
                foreach (var VARIABLE in desencryptedArray)
                {
                    tempPass += VARIABLE;
                }

                desencryptedPasswords[j] = tempPass;
                j++;
            }
        }

    }
}
