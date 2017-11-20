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
