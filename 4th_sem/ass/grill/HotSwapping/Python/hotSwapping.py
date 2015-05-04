import imp
import importlib
import sys
from importedModule import ModuleClass

def main():
   while True:
      inputValue = input("Run again? (y/ n): ")
      if inputValue == "n":
          break;
      # dynamically load module
      module = imp.load_source("importedModule", "./importedModule.py")
      module.ModuleClass.doSomething()
      # Does not work with this method
      ModuleClass.doSomething()
      # Does not work with this method
      test = importlib.reload(sys.modules['importedModule']).ModuleClass
      test.doSomething()

if  __name__ =='__main__':main()