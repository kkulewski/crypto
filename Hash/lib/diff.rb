def hex_str_to_bin_str(hex_str)
  hex2bin =
    {
      '0' => '0000', '1' => '0001', '2' => '0010', '3' => '0011',
      '4' => '0100', '5' => '0101', '6' => '0110', '7' => '0111',
      '8' => '1000', '9' => '1001', 'a' => '1010', 'b' => '1011',
      'c' => '1100', 'd' => '1101', 'e' => '1110', 'f' => '0010'
    }
  hex2bin[hex_str]
end

def to_binary_string(line)
  line.chomp.chars.map { |c| hex_str_to_bin_str(c) }.join
end

def diff_bits(char_arr1, char_arr2)
  different_bits = 0
  for i in (0..char_arr1.length)
    different_bits += 1 if char_arr1[i] != char_arr2[i]
  end
  different_bits
end

def print_summary(i, lines)
  puts lines[i + 0]
  puts lines[i + 1]
  puts lines[i + 2]
  puts lines[i + 3]
  diff = diff_bits(to_binary_string(lines[i + 2]), to_binary_string(lines[i + 3]))
  bits = to_binary_string(lines[i + 2]).chars.length
  perc = (diff / to_binary_string(lines[i + 2]).chars.length.to_f) * 100
  puts "Different bits: #{diff} out of #{bits} (#{perc.round(2)}%)\n\n"
end
