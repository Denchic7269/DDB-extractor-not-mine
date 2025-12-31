using System;
using System.IO;
using System.Text;

namespace ddbextractor
{
    class Program
    {
		
        static void Main(string[] args)
        {
			//SND (53 4E 44) (83 78 68)
			//FRM2h, (46 52 4D 32 68 2C) (70 82 77 50 104 44)
			//the position in the array where the sample starts, after SND
			int startPosition = -1;
			//the position in the array where the sample ends, after FRM2h,
			int endPosition = -1;
			//keeps track of what sample we're on; used for naming output files
			int sampleNumber = 0;
			//reads the bytes from the file into an array
			string argsString = String.Concat(args);
            byte[] array = File.ReadAllBytes(argsString);
			Console.WriteLine(array.Length);

			

				//searches the array until SND and FRM2h, are found
				for (int arrayPosition = 0; arrayPosition < array.Length/* && (startPosition == -1 || endPosition == -1)*/; arrayPosition++)
				{
					//If the current byte = S and following bytes are N and D, set byte after SND to start position
					if (array[arrayPosition] == 83 && array[arrayPosition + 1] == 78 && array[arrayPosition + 2] == 68 && startPosition == -1)
					{
						startPosition = arrayPosition;
						Console.WriteLine("Found beginning of sample");
						//Console.WriteLine(Convert.ToChar(array[arrayPosition]).ToString());
					}
					//Same thing but with FRM2h,
					if (array[arrayPosition] == 70 && array[arrayPosition +1] == 82 && array[arrayPosition + 2] == 77 && array[arrayPosition + 3] == 50 /*&& array[arrayPosition + 4] == 104 && array[arrayPosition + 5] == 44 */&& startPosition != -1)
					{
						endPosition = arrayPosition - 1;
						Console.WriteLine("Found end of sample");					
					}
					if (startPosition != -1 && endPosition != -1)
					{
					

						sampleNumber = sampleNumber + 1;
						Console.WriteLine(sampleNumber);
						Console.WriteLine("Found start and end of sample");
						Console.WriteLine(endPosition - startPosition);
						byte[] sampleArray = new byte[endPosition - startPosition];
						Array.Copy(array, startPosition, sampleArray, 0, endPosition - startPosition);
						//make a new file
						using (FileStream file = new FileStream($"Output{sampleNumber}", FileMode.Create))
						{
							//writing bytes to file
							Console.WriteLine("Writing bytes to file");
							for(int i = 0; i <sampleArray.Length; i++)
							{
								file.WriteByte(sampleArray[i]);
							}
						}
						startPosition = -1;
						endPosition = -1;
						
					}
				}
			
			
        }
    }
}
