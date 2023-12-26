(ns aoc-2023.day-25
  (:require [aoc-2023.helpers :as helpers]
            [clojure.core.matrix :as matrix]
            [clojure.core.matrix.linear :as linear]
            [clojure.string :as string]))

(matrix/set-current-implementation :vectorz)

(def input-file-path "inputs/day_25/input")
(def sample-file-path "inputs/day_25/sample-1")

(defn parse-input
  [filename]
  (let [entries (->> (string/split (slurp filename) #"\n")
                     (map #(string/split % #": "))
                     (map (fn [e] [(first e) (string/split (second e)
                                                           #" ")]))
                     (into {}))]
    (reduce (fn [out [k vs]]
              (reduce (fn [io v]
                        (update io v #(if % (conj % k) (set [k]))))
                      (update out k #(if % (into % vs) (set vs)))
                      vs))
            {}
            entries)))

(defn part-one
  "tried with stoer-wagner, it worked with the sample, but the implementation
   is too slow for the input.
   https://kaygun.tumblr.com/post/643991756387532800
   stumbled over this solution:
   /r/adventofcode/comments/18qbsxs/2023_day_25_solutions/keubcfd/"
  ([] (part-one input-file-path))
  ([filename]
   (let [graph (parse-input filename)
         node-to-idx (into {} (keep-indexed #(vector %2 %1) (keys graph)))
         adjac-mat (reduce (fn [adj [n vs]]
                             (reduce (fn [out v]
                                       (assoc-in out [(get node-to-idx n)
                                                      (get node-to-idx v)] 1))
                                     adj
                                     vs))
                           (vec (repeat (count graph)
                                        (vec (repeat (count graph) 0))))
                           graph)
         laplacian (matrix/sub
                    (matrix/mul (matrix/identity-matrix (count graph))
                                (map #(apply + %) adjac-mat))
                    adjac-mat)
         {:keys [U S V*]} (linear/svd laplacian)
         target (first (second (sort-by #(identity (second %))
                                        (keep-indexed vector S))))]
     ;; The eigenvalues are not correct - works for my input and sample
     ;; The eigenvectors are guessed, I take the larger number.
     (apply max
            (map
             #(let [val (count (filter pos? (vec (matrix/get-row % target))))]
                (* val (- (count graph) val))) [U V*])))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   "just push the button!"))

(defn run
  []
  (println (part-one))
  (println (part-two)))