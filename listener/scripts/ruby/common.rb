require 'yaml'

class Config
  PATH = 'config.yml'

  def initialize
    @config = YAML.load_file(File.join(File.dirname(__FILE__), PATH))
  end

  def [] idx
    @config[idx]
  end
end

