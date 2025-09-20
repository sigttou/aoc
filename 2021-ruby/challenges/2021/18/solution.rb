# frozen_string_literal: true

module Year2021
  class Day18 < Solution
    class Node
      def initialize(parent)
        @parent = parent
      end

      attr_accessor :left, :right, :val, :parent

      def print
        l = left.is_a?(Node) ? left.print : left
        r = right.is_a?(Node) ? right.print : right
        "[#{l},#{r}]"
      end

      def magnitude
        val = 3 * (left.is_a?(Node) ? left.magnitude : left)
        val + 2 * (right.is_a?(Node) ? right.magnitude : right)
      end
    end

    class Calculator
      def initialize
      end

      def parse(str)
        iterator = str.each_char
        parse_str(nil, iterator).tap do |node|
          raise "parse error" unless node.print == str
        end
      end

      def print
        val.print
      end

      def add(str)
        add_node(parse(str))
        reduce
      end

      def reduce
        loop do
          self.exploded = false
          explode(val, 0)
          next if exploded
          self.splited = false
          split(val)
          break unless splited
        end
      end

      def magnitude
        val.magnitude
      end

      private

      attr_accessor :val, :exploded, :splited

      def add_node(other)
        if val.nil?
          self.val = other
          return
        end
        node = Node.new(nil)
        node.left = val
        node.right = other
        val.parent = node
        other.parent = node
        self.val = node
      end

      def parse_str(parent, iterator)
        char = iterator.next
        char = iterator.next while char == "," || char == "]"

        if char == "["
          node = Node.new(parent)
          node.left = parse_str(node, iterator)
          node.right = parse_str(node, iterator)
          node
        else
          char.to_i
        end
      end

      def explode(node, depth)
        return if exploded
        return unless node.is_a?(Node)

        if depth == 4
          inc_left(node.left, node)
          inc_right(node.right, node)
          if node.parent.left == node
            node.parent.left = 0
          else
            node.parent.right = 0
          end
          self.exploded = true
        else
          explode(node.left, depth + 1)
          explode(node.right, depth + 1)
        end
      end

      def inc_right(value, node)
        node = next_right(node)
        return if node.nil?

        if node.right.is_a?(Node)
          node = node.right
          node = node.left while node.left.is_a?(Node)
          node.left += value
        else
          node.right += value
        end
      end

      def next_right(node)
        parent = node.parent
        while parent&.right == node
          node = parent
          parent = node.parent
        end
        parent
      end

      def inc_left(value, node)
        node = prev_left(node)
        return if node.nil?

        if node.left.is_a?(Node)
          node = node.left
          node = node.right while node.right.is_a?(Node)
          node.right += value
        else
          node.left += value
        end
      end

      def prev_left(node)
        parent = node.parent
        while parent&.left == node
          node = parent
          parent = node.parent
        end
        parent
      end

      def split(node)
        split_left(node) || split_right(node)
      end

      def split_left(node)
        return if splited

        if node.left.is_a?(Node)
          split(node.left)
        elsif node.left >= 10
          new_node = Node.new(node)
          new_node.left = node.left / 2
          new_node.right = (node.left + 1) / 2
          node.left = new_node
          self.splited = true
        end
      end

      def split_right(node)
        return if splited

        if node.right.is_a?(Node)
          split(node.right)
        elsif node.right >= 10
          new_node = Node.new(node)
          new_node.left = node.right / 2
          new_node.right = (node.right + 1) / 2
          node.right = new_node
          self.splited = true
        end
      end
    end

    def part_1(input = nil)
      lines = input ? input.strip.split("\n") : data.map(&:to_s)

      c = Calculator.new
      lines.each { |line| c.add(line) }
      c.magnitude
    end

    def part_2(input = nil)
      lines = input ? input.strip.split("\n") : data.map(&:to_s)

      mags = lines.permutation(2).map do |a, b|
        c = Calculator.new
        c.add(a)
        c.add(b)
        c.magnitude
      end
      mags.max
    end

    private

    # Processes each line of the input file and stores the result in the dataset
    def process_input(line)
      line  # Keep as string for parsing
    end

    # Processes the dataset as a whole
    def process_dataset(set)
      set
    end
  end
end
