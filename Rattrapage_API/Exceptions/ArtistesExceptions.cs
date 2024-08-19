namespace Rattrapage_API.Exceptions
{
    public class ArtisteNotFoundException : Exception
    {
        public ArtisteNotFoundException(string id)
            : base($"L'artiste avec l'ID {id} n'a pas été trouvé.")
        {
        }
    }

    public class ArtistesListNotFoundException : Exception
    {
        public ArtistesListNotFoundException() 
            : base($"Erreur lors de la récupération des artistes.")
        {
        }
    }

    public class ArtisteAlreadyExistsException : Exception
    {
        public ArtisteAlreadyExistsException(string name)
            : base($"Un artiste avec le nom {name} existe déjà.")
        {
        }
    }

    public class FileReadException : Exception
    {
        public FileReadException(string filePath)
            : base($"Erreur lors de la lecture du fichier : {filePath}.")
        {
        }
    }

    public class FileWriteException : Exception
    {
        public FileWriteException(string filePath)
            : base($"Erreur lors de l'écriture dans le fichier : {filePath}.")
        {
        }
    }

    public class FileNotFoundException : Exception
    {
        public FileNotFoundException(string filePath)
            : base($"Fichier non trouvé : {filePath}.")
        {
        }
    }
}
