using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Reflection;
using UnitTestEx;
using Assert = NUnit.Framework.Assert;

namespace UnitTestProject
{
    /// <summary>
    /// Summary description for FileStorageTest
    /// </summary>
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
        public const string WRONG_SIZE_CONTENT_STRING = "TEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtext";
        public const string TIC_TOC_TOE_STRING = "tictoctoe.game";

        public const int NEW_SIZE = 5;

        public FileStorage storage = new FileStorage(NEW_SIZE);

        /* ПРОВАЙДЕРЫ */

        static object[] NewFilesData =
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING) },
            new object[] { new File(FILE_PATH_STRING, CONTENT_STRING) }
        };

        static object[] WrongSizeFile =
        {
            new object[] { new File(SPACE_STRING, WRONG_SIZE_CONTENT_STRING) }
        };
        
        static object[] FileForDeleteData =
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING), REPEATED_STRING }
        };

        static object[] NullNameFileForDeleteData =
        {
            new object[] { new File() }
        };

        static object[] NewExceptionFileData = 
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING) }
        };

        [SetUp]
        public void Setup() // Очистка ПЕРЕД каждым тестом
        {
            storage.DeleteAllFiles(); 
        }

        /* Тестирование записи файла */
        [Test, TestCaseSource(nameof(NewFilesData))]
        public void WriteTest(File file) 
        {
            Assert.True(storage.Write(file));
        }
        /* Тестирование записи дублирующегося файла */
        [Test, TestCaseSource(nameof(NewExceptionFileData))]
        public void WriteExceptionTest(File file) {
            bool isException = false;
            try
            {
                storage.Write(file);
                Assert.False(storage.Write(file));
            } 
            catch (FileNameAlreadyExistsException)
            {
                isException = true;
            }
            Assert.True(isException, NO_EXPECTED_EXCEPTION_EXCEPTION);
        }

        /* Тестирование проверки существования файла */
        [Test, TestCaseSource(nameof(NewFilesData))]
        public void IsExistsTest(File file) 
        {
            String name = file.GetFilename();
            Assert.False(storage.IsExists(name));
            try 
            {
                storage.Write(file);
            } 
            catch (FileNameAlreadyExistsException e) 
            {
                Console.WriteLine(String.Format("Exception {0} in method {1}", e.GetBaseException(), MethodBase.GetCurrentMethod().Name));
            }
            Assert.True(storage.IsExists(name));
            storage.DeleteAllFiles();
        }

        /* Тестирование удаления файла */
        [Test, TestCaseSource(nameof(FileForDeleteData))]
        public void DeleteTest(File file, String fileName) 
        {
            storage.Write(file);
            Assert.True(storage.Delete(fileName));
        }
        [Test, TestCaseSource(nameof(NullNameFileForDeleteData))]
        public void DeleteTestNullName(File file)
        {
            storage.Write(file);
            Assert.IsFalse(storage.Delete(file.GetFilename()));
        }

        /* Тестирование получения файлов */
        [Test]
        public void GetFilesTest()
        {
            foreach (File el in storage.GetFiles()) 
            {
                Assert.NotNull(el);
            }
        }

        // Почти эталонный
        /* Тестирование получения файла */
        [Test, TestCaseSource(nameof(NewFilesData))]
        public void GetFileTest(File expectedFile) 
        {
            storage.Write(expectedFile);

            File actualfile = storage.GetFile(expectedFile.GetFilename());
            bool difference = actualfile.GetFilename().Equals(expectedFile.GetFilename()) && actualfile.GetSize().Equals(expectedFile.GetSize());

            Assert.IsTrue(difference, string.Format("There is some differences in {0} or {1}", expectedFile.GetFilename(), expectedFile.GetSize()));
        }

        [Test, TestCaseSource(nameof(WrongSizeFile))]
        public void WriteWrongSizeFile(File file)
        {
            Assert.False(storage.Write(file));
        }
    }
}
