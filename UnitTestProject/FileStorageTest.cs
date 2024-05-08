using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using UnitTestEx;

namespace UnitTestProject
{
    [TestClass]
    public class FileStorageTest
    {
        public const string MAX_SIZE_EXCEPTION = "DIFFERENT MAX SIZE";
        public const string NULL_FILE_EXCEPTION = "NULL FILE";
        public const string NO_EXPECTED_EXCEPTION_EXCEPTION = "There is no expected exception";

        public const string SPACE_STRING = " ";
        public const string FILE_PATH_STRING = "@D:\\JDK-intellij-downloader-info.txt";
        public const string CONTENT_STRING = "Some text";
        public const string REPEATED_STRING = "AA";
        public const string WRONG_SIZE_CONTENT_STRING = "TEXTtextTEXTtext..."; // shortened for brevity
        public const string TIC_TOC_TOE_STRING = "tictoctoe.game";

        public const int NEW_SIZE = 5;

        public FileStorage storage = new FileStorage(NEW_SIZE);

        /* ПРОВАЙДЕРЫ */

        static object[] NewFilesData =
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING) },
            new object[] { new File(SPACE_STRING, WRONG_SIZE_CONTENT_STRING) },
            new object[] { new File(FILE_PATH_STRING, CONTENT_STRING) }
        };

        static object[] FilesForDeleteData =
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING), REPEATED_STRING },
            new object[] { null, TIC_TOC_TOE_STRING }
        };

        static object[] NewExceptionFileData = {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING) }
        };


        /* Тестирование удаления файла */
        /*
    @Test(dataProvider = "testFilesForDelete", dependsOnMethods = "testFileStorage")
    public void testDelete(String fileName) throws FileNameAlreadyExistsException
    {
        File fileToDelete = new File(fileName, "Content"); // Создаем файл для удаления
        storage.write(fileToDelete); // Добавляем файл в хранилище
        Assert.assertTrue(storage.delete(fileName)); // Удаляем файл из хранилища
    } */
        [TestMethod]
        [DataRow(null, TIC_TOC_TOE_STRING)]
        public void DeleteTest(string filePath, string fileName)
        {
            File file = null;
            if (filePath != null)
            {
                file = new File(filePath, CONTENT_STRING);
                storage.Write(file);
            }
            Assert.IsFalse(storage.Delete(fileName), $"Failed to delete file with name '{fileName}'");
        }

        /* Тестирование записи файла */
        [TestMethod]
        [DataRow(REPEATED_STRING, CONTENT_STRING)]
        [DataRow(SPACE_STRING, WRONG_SIZE_CONTENT_STRING)]
        [DataRow(FILE_PATH_STRING, CONTENT_STRING)]
        public void WriteTest(string filePath, string fileContent)
        {
            File file = new File(filePath, fileContent);
            Assert.IsTrue(storage.Write(file));
            storage.DeleteAllFiles();
        }

        /* Тестирование записи дублирующегося файла */
        [TestMethod]
        [DataRow(REPEATED_STRING, CONTENT_STRING)]
        public void WriteExceptionTest(string filePath, string fileContent)
        {
            File file = new File(filePath, fileContent);
            bool isException = false;
            try
            {
                storage.Write(file);
                Assert.IsFalse(storage.Write(file));
                storage.DeleteAllFiles();
            }
            catch (FileNameAlreadyExistsException)
            {
                isException = true;
            }
            Assert.IsTrue(isException, NO_EXPECTED_EXCEPTION_EXCEPTION);
        }

        /* Тестирование проверки существования файла */
        [TestMethod]
        [DataRow(REPEATED_STRING, CONTENT_STRING)]
        [DataRow(SPACE_STRING, WRONG_SIZE_CONTENT_STRING)]
        [DataRow(FILE_PATH_STRING, CONTENT_STRING)]
        public void IsExistsTest(string filePath, string fileContent)
        {
            File file = new File(filePath, fileContent);
            string name = file.GetFilename();
            Assert.IsFalse(storage.IsExists(name));
            try
            {
                storage.Write(file);
            }
            catch (FileNameAlreadyExistsException e)
            {
                Console.WriteLine($"Exception {e.GetBaseException()} in method {MethodBase.GetCurrentMethod().Name}");
            }
            Assert.IsTrue(storage.IsExists(name));
            storage.DeleteAllFiles();
        }


        /* Тестирование получения файлов */
        [TestMethod]
        [DataRow(REPEATED_STRING, CONTENT_STRING)]
        [DataRow(SPACE_STRING, WRONG_SIZE_CONTENT_STRING)]
        [DataRow(FILE_PATH_STRING, CONTENT_STRING)]
        public void GetFilesTest(string filePath, string fileContent)
        {
            File file = new File(filePath, fileContent);
            foreach (File el in storage.GetFiles())
            {
                Assert.IsNotNull(el);
            }
        }

        // Почти эталонный
        /* Тестирование получения файла */
        [TestMethod]
        [DataRow(REPEATED_STRING, CONTENT_STRING)]
        [DataRow(SPACE_STRING, WRONG_SIZE_CONTENT_STRING)]
        [DataRow(FILE_PATH_STRING, CONTENT_STRING)]
        public void GetFileTest(string filePath, string fileContent)
        {
            File expectedFile = new File(filePath, fileContent);
            storage.Write(expectedFile);

            File actualFile = storage.GetFile(expectedFile.GetFilename());

            Assert.AreEqual(expectedFile.GetSize(), actualFile.GetSize(), $"There is some differences in {expectedFile.GetFilename()} size");

        }

        /* Тестирование получения файла, когда файл отсутствует */
        [TestMethod]
        public void GetFileTest_FileNotExists_ReturnsNull()
        {
            string fileName = "nonexistent_file.txt";
            File file = storage.GetFile(fileName);
            Assert.IsNull(file);
        }

        /* Тестирование записи файла, когда доступное пространство исчерпано */
        [TestMethod]
        public void WriteTest_StorageFull_ReturnsFalse()
        {
            FileStorage limitedStorage = new FileStorage(5);
            File largeFile = new File("large_file.txt", "scdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscsscdscsdcsdccdscs");

            bool result = limitedStorage.Write(largeFile);
            Assert.IsFalse(result);
        }

        /* Тестирование удаления файла, когда файл не найден */
        [TestMethod]
        public void DeleteTest_FileNotExists_ReturnsFalse()
        {
            string fileName = "nonexistent_file.txt";
            bool result = storage.Delete(fileName);
            Assert.IsFalse(result);
        }


    }
}
