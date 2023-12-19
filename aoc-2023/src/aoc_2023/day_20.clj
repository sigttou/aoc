(ns aoc-2023.day-20
  (:require [aoc-2023.helpers :as helpers]
            [clojure.math.numeric-tower :as math]
            [clojure.string :as string]))

(def input-file-path "inputs/day_20/input")
(def sample-file-path "inputs/day_20/sample-1")
(def sample-2-file-path "inputs/day_20/sample-2")

(defn parse-input
  [filename]
  (let [entries (map #(string/split % #" -> ")
                     (string/split (slurp filename) #"\n"))
        modules (reduce (fn [out entry]
                          (let [[mtype name]
                                (if (= \% (first (first entry)))
                                  [:flip (apply str (rest (first entry)))]
                                  (if (= \& (first (first entry)))
                                    [:conj (apply str (rest (first entry)))]
                                    [:broadcast (first entry)]))
                                outputs (string/split (second entry) #", ")]
                            (assoc out name {:type mtype
                                             :outputs outputs})))
                        {}
                        entries)]
    modules))

(defn get-inputs
  [modules]
  (reduce (fn [outm [name module]]
            (reduce (fn [out output]
                      (assoc out output (conj (get out output) name)))
                    outm
                    (:outputs module)))
          {}
          modules))

(defn get-history
  [modules inputs]
  (reduce (fn [outm [name module]]
            (if (= (:type module) :flip)
              (assoc outm name false)
              (if (= (:type module) :conj)
                (assoc outm name (reduce #(assoc %1 %2 false)
                                         {} (get inputs name)))
                outm)))
          {}
          modules))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [modules (parse-input filename)
         inputs (get-inputs modules)
         hcnt (atom 0)
         lcnt (atom 0)]
     (loop [cnt 0
            hist (get-history modules inputs)]
       (if (< cnt 1000)
         (recur
          (inc cnt)
          (loop [[new-hist todo] [hist [[nil "broadcaster" false]]]]
            (if (empty? todo)
              new-hist
              (recur
               (reduce
                (fn [[lhist ltodo] [src name high]]
                  (if high
                    (swap! hcnt inc)
                    (swap! lcnt inc))
                  (if-let [node (get modules name)]
                    (case (:type node)
                      :flip (if high
                              [lhist ltodo]
                              (let [state (get lhist name)]
                                [(assoc lhist name (not state))
                                 (concat ltodo
                                         (mapv #(identity
                                                 [name % (not state)])
                                               (:outputs node)))]))
                      :conj (let [states (get lhist name)
                                  nstates (assoc states src high)
                                  signal (if (some false? (vals nstates))
                                           true
                                           false)]
                              [(assoc lhist name nstates)
                               (concat ltodo (mapv #(identity
                                                     [name % signal])
                                                   (:outputs node)))])
                      :broadcast [lhist (concat ltodo
                                                (mapv #(identity
                                                        [name % high])
                                                      (:outputs node)))])
                    [lhist ltodo]))
                [new-hist []]
                todo)))))
         nil))
     (* @hcnt @lcnt))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [modules (parse-input filename)
         inputs (get-inputs modules)
         sources (get inputs (first (get inputs "rx")))
         counts (atom (into {} (map #(identity [% 0]) sources)))]
     (reduce math/lcm
      (loop [cnt 1
             hist (get-history modules inputs)]
        (if (some zero? (vals @counts))
          (recur
           (inc cnt)
           (loop [[new-hist todo] [hist [[nil "broadcaster" false]]]]
             (if (empty? todo)
               new-hist
               (recur
                (reduce
                 (fn [[lhist ltodo] [src name high]]
                   (if (and (some #{name} sources)
                            (not high)
                            (= 0 (get @counts name)))
                     (swap! counts #(assoc % name cnt))
                     :default)
                   (if-let [node (get modules name)]
                     (case (:type node)
                       :flip (if high
                               [lhist ltodo]
                               (let [state (get lhist name)]
                                 [(assoc lhist name (not state))
                                  (concat ltodo
                                          (mapv #(identity
                                                  [name % (not state)])
                                                (:outputs node)))]))
                       :conj (let [states (get lhist name)
                                   nstates (assoc states src high)
                                   signal (if (some false? (vals nstates))
                                            true
                                            false)]
                               [(assoc lhist name nstates)
                                (concat ltodo (mapv #(identity
                                                      [name % signal])
                                                    (:outputs node)))])
                       :broadcast [lhist (concat ltodo
                                                 (mapv #(identity
                                                         [name % high])
                                                       (:outputs node)))])
                     [lhist ltodo]))
                 [new-hist []]
                 todo)))))
          (vals @counts)))))))

(defn run
  []
  (println (part-one))
  (println (part-two)))