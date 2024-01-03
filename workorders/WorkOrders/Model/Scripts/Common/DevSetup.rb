require 'Win32API'
require 'singleton'

class Gui
  BUTTONS_OK = 0

  include Singleton

  class << self
    def message_box *args
      instance.message_box *args
    end
  end

  def message_box txt, title, buttons = BUTTONS_OK
    msgbox_fn.call 0, txt, title, buttons
  end

  private

  def msgbox_fn
    Win32API.new('user32', 'MessageBox', ['i', 'p', 'p', 'i'], 'i')
  end
end

APP_NAME = 'DevSetup'
OUTPUT_FILE = 'DevSetup.sql'

SCRIPT_FILES = {
  :drop => '01 DropDatabase.sql',
  :existing_tables => '02 CreateExistingTables.sql',
  :create => '03 CreateDatabase.sql',
  :site_data => '04 ImportSiteData.sql',
  :static_data => '05 CreateStaticData.sql',
  :op_cntr_data => '06 CreateOperatingCenterData.sql',
  :sample_data => '07 CreateSampleData.sql'
}

# used to specify the order in which the script files should be read and
# appended to the output script:
SCRIPT_ARR = [SCRIPT_FILES[:drop],
              SCRIPT_FILES[:existing_tables],
              SCRIPT_FILES[:create],
              SCRIPT_FILES[:site_data],
              SCRIPT_FILES[:static_data],
              SCRIPT_FILES[:op_cntr_data],
              SCRIPT_FILES[:sample_data]]

DATABASE_NAME = 'WorkOrdersTest'

def message_box txt
  Gui.message_box txt, APP_NAME
end

out_file = File.new OUTPUT_FILE, 'w'

out_file.puts "USE [#{DATABASE_NAME}]"
out_file.puts 'GO'

SCRIPT_ARR.each do |script|
  in_file = File.new script

  out_file.puts "RAISERROR (N'SCRIPT FILE ''#{script}'' PROCESSING...', 10, 1) WITH NOWAIT;"
  out_file.puts 'GO'
  out_file.puts in_file.read

  in_file.close
end

out_file.close

puts "Finished concatenating SQL script files.  Now processing script '#{OUTPUT_FILE}'."

# swap these if you need to see the output
system 'cmd /C "osql -E -i ' + OUTPUT_FILE + '"'
#system 'cmd /K "osql -E -i ' + OUTPUT_FILE + '"'

message_box 'Finished executing dev setup script.'
