file = open("../data_files/test_files/numbers1", "w")
for number in range(1001,10000):
  file.write(str(number))
  file.write("\n")
file.close()
