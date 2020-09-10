#include <iostream>
#include <fstream>
#include <string>
#include <vector>

std::vector<bool> first(10);
static std::vector<std::string> outputNames{
  ".\\ClassFiles\\neutral.json",
  ".\\ClassFiles\\druid.json",
  ".\\ClassFiles\\hunter.json",
  ".\\ClassFiles\\mage.json",
  ".\\ClassFiles\\paladin.json",
  ".\\ClassFiles\\priest.json",
  ".\\ClassFiles\\rogue.json",
  ".\\ClassFiles\\shaman.json",
  ".\\ClassFiles\\warlock.json",
  ".\\ClassFiles\\warrior.json"
};
enum Classes{
  NEUTRAL,
  DRUID,
  HUNTER,
  MAGE,
  PALADIN,
  PRIEST,
  ROGUE,
  SHAMAN,
  WARLOCK,
  WARRIOR,
  ERROR
};
void OpenAllClassFiles(std::vector<std::ofstream>& outputFiles)
{
  // Open up all of the output files
  int i = 0;
  for (auto& path : outputNames)
  {
    outputFiles.push_back(std::ofstream());
    outputFiles[i].open(path);
    outputFiles[i++] << "[";
  }
}

void CloseAllClassFiles(std::vector<std::ofstream>& outputFiles)
{
  for (auto& file : outputFiles)
  {
    file << "]";
    file.close();
  }
}

void ReadJSONFileIntoString(const char* filename, std::string& output)
{
  // Open file
  std::ifstream cards(filename, std::fstream::in);

  // Grab each character of the file and push it back into the string
  if (cards.is_open())
  {
    char current;
    while (cards >> current)
      output.push_back(current);

    cards.close();
  }
  else
    std::cout << "Error reading card collection" << std::endl;
}

Classes FindClass(std::string& cards, std::size_t start)
{
  // Find the start of the class
  std::size_t startOfClass = cards.find("cardClass", start);
  startOfClass = cards.find(":", startOfClass) + 2;
  std::size_t endOfClass = cards.find("\"", startOfClass);
  std::string theClass = cards.substr(startOfClass, endOfClass - startOfClass);
  //std::cout << theClass << std::endl;

  // Return the correct
  if(theClass == "NEUTRAL")
    return NEUTRAL;
  else if(theClass == "HUNTER")
    return HUNTER;
  else if(theClass == "MAGE")
    return MAGE;
  else if(theClass == "PALADIN")
    return PALADIN;
  else if(theClass == "PRIEST")
    return PRIEST;
  else if(theClass == "ROGUE")
    return ROGUE;
  else if(theClass == "SHAMAN")
    return SHAMAN;
  else if(theClass == "WARLOCK")
    return WARLOCK;
  else if(theClass == "WARRIOR")
    return WARRIOR;  
  else if(theClass == "DRUID")
    return DRUID;

  return ERROR;
}

void ParseClasses(std::string &cards, std::vector<std::ofstream> &outputFiles)
{
  // Start at the beginning of the string
  std::size_t currentPosition = 0;

  while(1)
  {
    // Find the start
    std::size_t startBracket = cards.find_first_of("{", currentPosition);

    // If we did not find one, then we are at the end
    if(startBracket == std::string::npos) break;

    // Figure out which file this card belongs to
    Classes currentClass = FindClass(cards, startBracket);

    if (currentClass == ERROR) 
      return; 

    if (first[currentClass])
    {
      outputFiles[currentClass] << ",";
    }
    else
    {
      first[currentClass] = true;
    }

    std::size_t endBracket = cards.find("type", startBracket);
    endBracket = cards.find_first_of("}", endBracket);
    std::string object = cards.substr(startBracket, endBracket - startBracket + 1);
    outputFiles[currentClass] << object;
    currentPosition = endBracket;
  }
}
int main(void)
{
  // Start reading the file and open up all of the other files
  std::vector<std::ofstream> outputFiles;
  OpenAllClassFiles(outputFiles);

  // Read in the JSON file
  std::string cards;
  ReadJSONFileIntoString(".//cards.collectible.json", cards);

  // Prase through files
  ParseClasses(cards, outputFiles);

  // Close and end all of our JSON files
  CloseAllClassFiles(outputFiles);

  return 0;
}