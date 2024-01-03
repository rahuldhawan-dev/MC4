def chunker f_in, ext = '', f_out = nil, chunksize = 3_340_032
  f_out ||= f_in
  outfilenum = 1
  File.open(f_in + ext,"r") do |fh_in|
    first_line = fh_in.readline
    until fh_in.eof?
      File.open("#{f_out}-#{outfilenum}#{ext}","w") do |fh_out|
        fh_out << first_line
        line = ""
        while fh_out.size <= (chunksize-line.length) && !fh_in.eof?
          line = fh_in.readline
          fh_out << line
        end
      end
      outfilenum += 1
    end
  end
end

chunker 'MapCall_PremiseMaster_20180830-003438-946', '.csv'
