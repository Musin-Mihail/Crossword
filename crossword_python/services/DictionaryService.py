import os
from crossword_python.models.Dictionary import Dictionary
from crossword_python.models.DictionaryWord import DictionaryWord

class DictionaryService:
    DICTIONARIES_FOLDER = "Dictionaries"

    def get_dictionary_paths(self) -> list[str]:
        if not os.path.exists(self.DICTIONARIES_FOLDER):
            os.makedirs(self.DICTIONARIES_FOLDER)
            return [ ]
        
        paths = [ ]
        for file in os.listdir(self.DICTIONARIES_FOLDER):
            if file.endswith(".txt"):
                paths.append(os.path.join(self.DICTIONARIES_FOLDER, file))
        return paths

    def load_dictionary(self, path: str) -> Dictionary:
        dictionary = Dictionary()
        with open(path, 'r', encoding='utf-8-sig') as f:
            lines = f.readlines()
            
        for line in lines:
            line = line.strip()
            if not line:
                continue
                
            parts = line.split(';')
            if not parts:
                continue
                
            dict_word = DictionaryWord(answers=parts[0], definitions=[ ])
            for i in range(1, len(parts)):
                dict_word.definitions.append(parts[i])
                
            dictionary.words.append(dict_word)
            
        return dictionary