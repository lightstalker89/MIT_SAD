import imp
import importlib
import sys
from importedModule import ModuleClass

def main():
   while True:
      inputValue = input("Want to execute program? (y/ n): ")
      if inputValue == "n":
          break;
      # dynamically load module
      dynamicExecute()
      # reload loaded module
      reloadExecute()

def dynamicExecute():
      # Dynamically read source before executing and assign it to variable module
      module = imp.load_source("importedModule", "./importedModule.py")
      # Execute method from loaded source
      module.ModuleClass.doSomething()

def reloadExecute():
      global ModuleClass
      # Reload already loaded ModuleClass
      ModuleClass = importlib.reload(sys.modules['importedModule']).ModuleClass
      # Execute method from imported module
      ModuleClass.doSomething()

if  __name__ =='__main__':main()